using CSM.Business.Core.SongSelection;
using CSM.Business.Interfaces;
using CSM.Business.Interfaces.SongCopy;
using CSM.DataAccess;
using CSM.DataAccess.BeatSaver;
using CSM.DataAccess.Playlists;
using CSM.DataAccess.UserConfiguration;
using CSM.Framework.Extensions;
using CSM.Framework.ServiceLocation;
using CSM.Framework.Types;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Controls;

namespace CSM.UiLogic.ViewModels.Common.Playlists
{
    internal sealed class PlaylistViewModel : BasePlaylistViewModel, IPlaylistViewModel
    {
        #region Private fields

        private IRelayCommand? fetchDataCommand, savePlaylistCommand, applySortOrderAndSaveCommand, chooseCoverImageCommand, updateFromSourceCommand;
        private PlaylistSongViewModel? selectedSong;
        private string sortColumnName = string.Empty;
        private GridViewSortingState sortingState = GridViewSortingState.None;
        private ViewDefinition? selectedViewDefinition;
        private bool isSongSuggest;

        private readonly SongSelectionType songSelectionType;
        private readonly Playlist playlist;
        private readonly ILogger logger;
        private readonly IBeatSaverService beatSaverService;
        private readonly ISongCopyDomain? songCopyDomain;
        private readonly ISongSelectionDomain songSelectionDomain;
        private readonly IUserConfigDomain userConfigDomain;

        #endregion

        #region Properties

        public IRelayCommand? FetchDataCommand => fetchDataCommand ??= CommandFactory.CreateFromAsync(FetchDataAsync, CanFetchData);
        public IRelayCommand? SavePlaylistCommand => savePlaylistCommand ??= CommandFactory.CreateFromAsync(SaveAsync, CanSave);
        public IRelayCommand? ApplySortOrderAndSaveCommand => applySortOrderAndSaveCommand ??= CommandFactory.CreateFromAsync(ApplySortOrderAndSaveAsync, CanSave);
        public IRelayCommand? ChooseCoverImageCommand => chooseCoverImageCommand ??= CommandFactory.CreateFromAsync(ChooseCoverImage, CanChooseCoverImage);
        public IRelayCommand? UpdateFromSourceCommand => updateFromSourceCommand ??= CommandFactory.CreateFromAsync(UpdateFromSourceAsync, CanUpdateFromSource);

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
            }
        }

        public bool HasSelectedSong
        {
            get => SelectedSong != null;
        }

        public string SongCount
        {
            get
            {
                if (Songs.Count == 0)
                    return "No songs";
                if (Songs.Count == 1)
                    return "1 song";
                return $"{Songs.Count} songs";
            }
        }

        public bool HasExternalSource
        {
            get
            {
                var syncUrl = playlist.CustomData?.SyncURL ?? string.Empty;
                if (string.IsNullOrWhiteSpace(syncUrl))
                {
                    syncUrl = playlist.syncURL ?? string.Empty;
                }
                return (!string.IsNullOrWhiteSpace(syncUrl));
            }
        }

        public ObservableCollection<ViewDefinition> ViewDefinitions { get; } = [];

        public ViewDefinition? SelectedViewDefinition
        {
            get => selectedViewDefinition;
            set
            {
                if (value == selectedViewDefinition)
                    return;
                selectedViewDefinition = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanSaveViewDefinition));
                OnPropertyChanged(nameof(CanDeleteViewDefinition));

                if (isSongSuggest)
                {
                    userConfigDomain!.Config!.PlaylistsConfig.LastSongSuggestViewDefinitionName = selectedViewDefinition?.Name;
                }
                else if (songSelectionType == SongSelectionType.Left)
                {
                    userConfigDomain!.Config!.PlaylistsConfig.LastLeftViewDefinitionName = selectedViewDefinition?.Name;
                }
                else
                {
                    userConfigDomain!.Config!.PlaylistsConfig.LastRightViewDefinitionName = selectedViewDefinition?.Name;
                }

                userConfigDomain.SaveUserConfig();
            }
        }

        public bool ShowViewDefinitions => ViewDefinitions.Count > 0;

        public bool CanSaveViewDefinition => SelectedViewDefinition != null;

        public bool CanDeleteViewDefinition => SelectedViewDefinition != null;

        public FilterMode FilterMode => userConfigDomain.Config?.FilterMode ?? FilterMode.PopUp;

        #endregion

        public PlaylistViewModel(
           IServiceLocator serviceLocator,
           Playlist playlist,
           string path,
           SongSelectionType songSelectionType,
           bool isReadOnly,
           bool isSongSuggest = false) : base(serviceLocator, playlist.PlaylistTitle, path)
        {
            this.playlist = playlist;
            this.songSelectionType = songSelectionType;
            this.isSongSuggest = isSongSuggest;
            logger = serviceLocator.GetService<ILogger<PlaylistViewModel>>();
            beatSaverService = serviceLocator.GetService<IBeatSaverService>();
            if (!isReadOnly)
            {
                songCopyDomain = serviceLocator.GetService<ISongCopyDomain>();
                songCopyDomain.OnCopySongs += SongCopyDomain_OnCopySongs;
            }
            songSelectionDomain = serviceLocator.GetService<ISongSelectionDomain>();
            userConfigDomain = serviceLocator.GetService<IUserConfigDomain>();

            foreach (var song in playlist.Songs)
            {
                var vm = new PlaylistSongViewModel(serviceLocator, song);
                if (!isReadOnly)
                {
                    vm.OnSongRemoved += Playlist_OnSongRemoved;
                }
                Songs.Add(vm);
            }
        }

        public async Task LoadAsync()
        {
            // Load view definitions
            List<ViewDefinition> viewDefinitions;
            string? lastViewDefinition;
            if (isSongSuggest)
            {
                viewDefinitions = await LoadViewDefinitionsAsync(SavableUiElement.SongSuggestPlaylist);
                lastViewDefinition = userConfigDomain!.Config?.PlaylistsConfig.LastSongSuggestViewDefinitionName;
            }
            else
            {
                viewDefinitions = await LoadViewDefinitionsAsync(songSelectionType == SongSelectionType.Right ? SavableUiElement.PlaylistRight : SavableUiElement.PlaylistLeft);
                lastViewDefinition = songSelectionType == SongSelectionType.Right ? userConfigDomain!.Config?.PlaylistsConfig.LastRightViewDefinitionName : userConfigDomain!.Config?.PlaylistsConfig.LastLeftViewDefinitionName;
            }

            ViewDefinitions.Clear();
            ViewDefinitions.AddRange(viewDefinitions);
            SelectedViewDefinition = ViewDefinitions.FirstOrDefault(vd => vd.Name == lastViewDefinition);
            OnPropertyChanged(nameof(ShowViewDefinitions));
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
                        var existingSongs = Songs.Where(s => string.Compare(s.Hash, mapDetail.Key, StringComparison.OrdinalIgnoreCase) == 0);
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
            if (SelectedSong == null)
                return;

            if (SelectedSong.MapDetailViewModel == null)
            {
                var mapDetail = await beatSaverService.GetMapDetailAsync(SelectedSong.Hash, BeatSaverKeyType.Hash);
                if (mapDetail == null)
                    return;

                SelectedSong.UpdateMapDetail(mapDetail);
            }

            // todo: which hash to use? latest? based on what?
            var hashes = SelectedSong.MapDetailViewModel?.Model.Versions.OrderBy(v => v.CreatedAt).Select(v => v.Hash).ToList();
            if (hashes != null && hashes.Count > 0)
                songSelectionDomain.SetSongHash(hashes.Last(), songSelectionType);
        }

        public override bool CheckContainsSong(string? hash, SongSelectionType songSelectionType)
        {
            var hasSelectedSong = Songs.Any(s => string.Compare(s.Hash, hash, StringComparison.OrdinalIgnoreCase) == 0);
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
            songSelectionDomain.SetSongHash(null, songSelectionType);
            foreach (var song in Songs)
            {
                song.CleanUpReferences();
                song.OnSongRemoved -= Playlist_OnSongRemoved;
            }

            if (songCopyDomain != null)
                songCopyDomain.OnCopySongs -= SongCopyDomain_OnCopySongs;
        }

        public void SetSortOrder(string columnName, GridViewSortingState sortingState)
        {
            sortColumnName = columnName;
            this.sortingState = sortingState;
        }

        public override async Task<ViewDefinition?> SaveViewDefinitionAsync(Stream stream, SavableUiElement savableUiElement, string? name = null)
        {
            var newViewDefinition = await base.SaveViewDefinitionAsync(stream, savableUiElement, name);
            if (newViewDefinition != null)
            {
                ViewDefinitions.Add(newViewDefinition);
                SelectedViewDefinition = newViewDefinition;
                OnPropertyChanged(nameof(ShowViewDefinitions));
            }
            return newViewDefinition;
        }

        public override void DeleteViewDefinition(SavableUiElement savableUiElement, string name)
        {
            if (SelectedViewDefinition == null)
                return;
            base.DeleteViewDefinition(savableUiElement, name);
            ViewDefinitions.Remove(SelectedViewDefinition);
            SelectedViewDefinition = ViewDefinitions.FirstOrDefault();
            OnPropertyChanged(nameof(ShowViewDefinitions));
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

        private async Task UpdateFromSourceAsync()
        {
            SetLoadingInProgress(true, "Updating playlist from source");

            var syncUrl = playlist.CustomData?.SyncURL ?? string.Empty;
            if (string.IsNullOrWhiteSpace(syncUrl))
            {
                syncUrl = playlist.syncURL ?? string.Empty;
            }
            if (string.IsNullOrWhiteSpace(syncUrl))
                return;

            try
            {
                using var client = new HttpClient();
                var content = await client.GetStringAsync(syncUrl);
                var newPlaylist = JsonSerializer.Deserialize<Playlist>(content, JsonSerializerHelper.CreateDefaultSerializerOptions());
                if (newPlaylist == null)
                    return;

                PlaylistTitle = newPlaylist.PlaylistTitle;
                PlaylistAuthor = newPlaylist.PlaylistAuthor;
                PlaylistDescription = newPlaylist.PlaylistDescription;
                playlist.Image = newPlaylist.Image;

                OnPropertyChanged(nameof(CoverImage));

                playlist.Songs.Clear();
                playlist.Songs.AddRange(newPlaylist.Songs);

                Songs.ForEach(s => s.OnSongRemoved -= Playlist_OnSongRemoved);
                Songs.ForEach(s => s.CleanUpReferences());
                Songs.Clear();
                foreach (var song in playlist.Songs)
                {
                    var vm = new PlaylistSongViewModel(ServiceLocator, song);
                    vm.OnSongRemoved += Playlist_OnSongRemoved;
                    Songs.Add(vm);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while updating playlist '{title}' from source.", playlist.PlaylistTitle);
                throw;
            }
            finally
            {
                SetLoadingInProgress(false, string.Empty);
            }
        }

        private bool CanUpdateFromSource()
        {
            var syncUrl = playlist.CustomData?.SyncURL ?? string.Empty;
            if (string.IsNullOrWhiteSpace(syncUrl))
            {
                syncUrl = playlist.syncURL ?? string.Empty;
            }
            return (!string.IsNullOrWhiteSpace(syncUrl));
        }

        private void SongCopyDomain_OnCopySongs(object? sender, Business.Core.SongCopy.SongCopyEventArgs e)
        {
            if (this != songCopyDomain?.SelectedPlaylist)
                return;

            if (e.OverwritePlaylist)
            {
                playlist.Songs.Clear();
                Songs.ForEach(s => s.OnSongRemoved -= Playlist_OnSongRemoved);
                Songs.ForEach(s => s.CleanUpReferences());
                Songs.Clear();
            }

            foreach (var songToCopy in e.Songs)
            {
                var existingSong = Songs.SingleOrDefault(s => s.Hash == songToCopy.Hash);
                if (existingSong != null)
                {
                    foreach (var difficultyToCopy in songToCopy.Difficulties ?? [])
                    {
                        var newDifficulty = new Difficulty
                        {
                            Characteristic = difficultyToCopy.Characteristic,
                            Name = difficultyToCopy.Name,
                        };
                        existingSong.AddDifficulty(newDifficulty);
                    }
                }
                else
                {
                    playlist.Songs.Add(songToCopy);
                    var playlistSongViewModel = new PlaylistSongViewModel(ServiceLocator, songToCopy);
                    playlistSongViewModel.OnSongRemoved += Playlist_OnSongRemoved;
                    Songs.Add(playlistSongViewModel);
                }
            }
            OnPropertyChanged(nameof(SongCount));
        }

        private void Playlist_OnSongRemoved(object? sender, EventArgs e)
        {
            if (sender is PlaylistSongViewModel playlistSongViewModel)
            {
                var existingSong = playlist.Songs.SingleOrDefault(s => s.Hash == playlistSongViewModel.Model.Hash);
                if (existingSong != null)
                {
                    playlist.Songs.Remove(existingSong);
                    playlistSongViewModel.CleanUpReferences();
                    playlistSongViewModel.OnSongRemoved -= Playlist_OnSongRemoved;
                    Songs.Remove(playlistSongViewModel);
                    OnPropertyChanged(nameof(SongCount));
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
