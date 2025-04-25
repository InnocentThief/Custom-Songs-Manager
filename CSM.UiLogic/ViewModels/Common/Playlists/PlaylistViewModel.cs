using CSM.Business.Core.SongSelection;
using CSM.Business.Interfaces;
using CSM.Business.Interfaces.SongCopy;
using CSM.DataAccess.BeatSaver;
using CSM.DataAccess.Playlists;
using CSM.Framework.ServiceLocation;
using CSM.Framework.Types;
using CSM.UiLogic.Commands;
using CSM.UiLogic.Helper;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Controls;

namespace CSM.UiLogic.ViewModels.Common.Playlists
{
    internal sealed class PlaylistViewModel : BasePlaylistViewModel, IPlaylistViewModel
    {
        #region Private fields

        private IRelayCommand? fetchDataCommand, savePlaylistCommand, applySortOrderAndSaveCommand, chooseCoverImageCommand;
        private PlaylistSongViewModel? selectedSong;
        private string sortColumnName = string.Empty;
        private GridViewSortingState sortingState = GridViewSortingState.None;

        private readonly SongSelectionType songSelectionType;
        private readonly Playlist playlist;
        private readonly ILogger logger;
        private readonly IBeatSaverService beatSaverService;
        private readonly ISongCopyDomain songCopyDomain;
        private readonly ISongSelectionDomain songSelectionDomain;
        private readonly IUserConfigDomain userConfigDomain;

        #endregion

        #region Properties

        public IRelayCommand? FetchDataCommand => fetchDataCommand ??= CommandFactory.CreateFromAsync(FetchDataAsync, CanFetchData);
        public IRelayCommand? SavePlaylistCommand => savePlaylistCommand ??= CommandFactory.CreateFromAsync(SaveAsync, CanSave);
        public IRelayCommand? ApplySortOrderAndSaveCommand => applySortOrderAndSaveCommand ??= CommandFactory.CreateFromAsync(ApplySortOrderAndSaveAsync, CanSave);
        public IRelayCommand? ChooseCoverImageCommand => chooseCoverImageCommand ??= CommandFactory.CreateFromAsync(ChooseCoverImage, CanChooseCoverImage);

        public string PlaylistTitle
        {
            get => playlist.PlaylistTitle;
            set
            {
                if (value == playlist.PlaylistTitle) return;
                playlist.PlaylistTitle = value;
                OnPropertyChanged();
            }
        }

        public string PlaylistAuthor
        {
            get => playlist.PlaylistAuthor;
            set
            {
                if (value == playlist.PlaylistAuthor) return;
                playlist.PlaylistAuthor = value;
                OnPropertyChanged();
            }
        }

        public string? PlaylistDescription
        {
            get => playlist.PlaylistDescription;
            set
            {
                if (value == playlist.PlaylistDescription) return;
                playlist.PlaylistDescription = value;
                OnPropertyChanged();
            }
        }

        public ImageSource? CoverImage
        {
            get
            {
                if (string.IsNullOrWhiteSpace(playlist.Image)) return null;
                return Converter.ImageConverter.BitmapFromBase64(playlist.Image.Split(',').Last());
            }
        }

        public ObservableCollection<PlaylistSongViewModel> Songs { get; } = [];

        public PlaylistSongViewModel? SelectedSong
        {
            get => selectedSong;
            set
            {
                if (value == selectedSong) return;
                selectedSong = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSelectedSong));

                songSelectionDomain.SetSongHash(selectedSong?.Hash ?? null, songSelectionType);
            }
        }

        public bool HasSelectedSong
        {
            get => SelectedSong != null;
        }

        #endregion

        public PlaylistViewModel(
           IServiceLocator serviceLocator,
           Playlist playlist,
           string path,
           SongSelectionType songSelectionType) : base(serviceLocator, playlist.PlaylistTitle, path)
        {
            this.playlist = playlist;
            this.songSelectionType = songSelectionType;
            logger = serviceLocator.GetService<ILogger<PlaylistViewModel>>();
            beatSaverService = serviceLocator.GetService<IBeatSaverService>();
            songCopyDomain = serviceLocator.GetService<ISongCopyDomain>();
            songCopyDomain.OnCopySongs += SongCopyDomain_OnCopySongs;
            songSelectionDomain = serviceLocator.GetService<ISongSelectionDomain>();
            userConfigDomain = serviceLocator.GetService<IUserConfigDomain>();

            foreach (var song in playlist.Songs)
            {
                var vm = new PlaylistSongViewModel(serviceLocator, song);
                vm.OnSongRemoved += Playlist_OnSongRemoved;
                Songs.Add(vm);
            }
        }

        public async Task FetchDataAsync()
        {
            SetLoadingInProgress(true, "Fetching and updating song data...");

            try
            {
                int totalPages = Songs.Count / 50 + 1;
                for (int i = 0; i < totalPages; i++)
                {
                    var songHashes = Songs.Skip(i * 50).Take(50).Select(s => s.Hash).ToList();
                    var mapDetails = await beatSaverService.GetMapDetailsAsync(songHashes, BeatSaverKeyType.Hash);
                    if (mapDetails == null)
                        continue;
                    foreach (var mapDetail in mapDetails)
                    {
                        if (mapDetail.Value == null)
                            continue;
                        var existingSongs = Songs.Where(s => s.Hash == mapDetail.Key);
                        foreach (var existingSong in existingSongs)
                        {
                            existingSong.UpdateData(mapDetail.Value);
                        }
                    }
                }
                await SaveAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching data for playlist {PlaylistTitle}", PlaylistTitle);
            }
            finally
            {
                SetLoadingInProgress(false, string.Empty);
            }
        }

        public async Task LoadSelectedSongDataAsync()
        {
            if (SelectedSong == null || SelectedSong.MapDetailViewModel != null)
                return;

            var mapDetail = await beatSaverService.GetMapDetailAsync(SelectedSong.Hash, BeatSaverKeyType.Hash);
            if (mapDetail == null)
                return;

            SelectedSong.UpdateMapDetail(mapDetail);
        }

        public override bool CheckContainsSong(string? hash, SongSelectionType songSelectionType)
        {
            var hasSelectedSong = Songs.Any(s => s.Hash == hash);
            if (songSelectionType == SongSelectionType.Left)
            {
                ContainsLeftSong = hasSelectedSong;
            }
            else
            {
                ContainsRightSong = hasSelectedSong;
            }
            return hasSelectedSong;
        }

        public override void CleanUpReferences()
        {
            songCopyDomain.OnCopySongs -= SongCopyDomain_OnCopySongs;
            foreach (var song in Songs)
            {
                song.OnSongRemoved -= Playlist_OnSongRemoved;
            }
        }

        public void SetSortOrder(string columnName, GridViewSortingState sortingState)
        {
            sortColumnName = columnName;
            this.sortingState = sortingState;
        }

        #region Helper methods

        private async Task SaveAsync()
        {
            var content = JsonSerializer.Serialize(playlist, JsonSerializerHelper.CreateDefaultSerializerOptions());
            await File.WriteAllTextAsync(Path, content);
        }

        private async Task ApplySortOrderAndSaveAsync()
        {
            if (string.IsNullOrWhiteSpace(sortColumnName)) return;
            if (sortingState == GridViewSortingState.None) return;
            var currentSongs = playlist.Songs.ToList();
            playlist.Songs.Clear();
            switch (sortColumnName)
            {
                case "BsrKeyHex":
                    if (sortingState == GridViewSortingState.Ascending)
                    {
                        playlist.Songs.AddRange(currentSongs.OrderBy(s => s.Key));
                    }
                    else
                    {
                        playlist.Songs.AddRange(currentSongs.OrderByDescending(s => s.Key));
                    }
                    break;
                case "SongName":
                    if (sortingState == GridViewSortingState.Ascending)
                    {
                        playlist.Songs.AddRange(currentSongs.OrderBy(s => s.SongName));
                    }
                    else
                    {
                        playlist.Songs.AddRange(currentSongs.OrderByDescending(s => s.SongName));
                    }
                    break;
                case "LevelAuthorName":
                    if (sortingState == GridViewSortingState.Ascending)
                    {
                        playlist.Songs.AddRange(currentSongs.OrderBy(s => s.LevelAuthorName));
                    }
                    else
                    {
                        playlist.Songs.AddRange(currentSongs.OrderByDescending(s => s.LevelAuthorName));
                    }
                    break;
                default:
                    break;
            }
            await SaveAsync();
        }

        private bool CanSave()
        {
            return true;
        }

        private bool CanFetchData()
        {
            return true;
        }

        private void SongCopyDomain_OnCopySongs(object? sender, Business.Core.SongCopy.SongCopyEventArgs e)
        {
            if (this != songCopyDomain.SelectedPlaylist) return;
            foreach (var song in e.Songs)
            {
                var existingSong = Songs.SingleOrDefault(s => s.Hash == song.Hash);
                if (existingSong != null) return;
                playlist.Songs.Add(song);
                var playlistSongViewModel = new PlaylistSongViewModel(ServiceLocator, song);
                playlistSongViewModel.OnSongRemoved += Playlist_OnSongRemoved;
                Songs.Add(playlistSongViewModel);
            }
        }

        private void Playlist_OnSongRemoved(object? sender, EventArgs e)
        {
            if (sender is PlaylistSongViewModel playlistSongViewModel)
            {
                var existingSong = playlist.Songs.SingleOrDefault(s => s.Hash == playlistSongViewModel.Model.Hash);
                if (existingSong != null)
                {
                    playlist.Songs.Remove(existingSong);
                    playlistSongViewModel.OnSongRemoved -= Playlist_OnSongRemoved;
                    Songs.Remove(playlistSongViewModel);
                }
            }
        }

        private async Task ChooseCoverImage()
        {
            var playlistPath = System.IO.Path.Combine(userConfigDomain.Config?.PlaylistsConfig.PlaylistPath.Path ?? "C:\\", "CoverImages");
            if (!Directory.Exists(playlistPath)) playlistPath = "C:\\";

            RadOpenFileDialog openFileDialog = new()
            {
                Owner = Application.Current.MainWindow,
                RestoreDirectory = true,
                InitialDirectory = playlistPath,
                Filter = "|Image Files|*.jpg;*.png"
            };
            openFileDialog.ShowDialog();
            if (openFileDialog.DialogResult == true)
            {
                playlist.Image = Converter.ImageConverter.StringFromBitmap(openFileDialog.FileName);
                await SaveAsync();
                OnPropertyChanged(nameof(CoverImage));
            }
        }

        private bool CanChooseCoverImage()
        {
            return true;
        }   

        #endregion
    }
}
