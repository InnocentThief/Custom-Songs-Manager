﻿using CSM.DataAccess.Entities.Offline;
using CSM.Framework.Configuration.UserConfiguration;
using CSM.Framework.Logging;
using CSM.Services;
using CSM.UiLogic.Properties;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Media;
using Telerik.Windows.Controls;
using AppCurrent = System.Windows.Application;
using CSMImageConverter = CSM.Framework.Converter.ImageConverter;

namespace CSM.UiLogic.Workspaces.Playlists
{
    /// <summary>
    /// Represents one playlist.
    /// </summary>
    public class PlaylistViewModel : BasePlaylistViewModel
    {
        #region Private fields

        private readonly Playlist playlist;
        private PlaylistSongViewModel playlistSong;
        private bool inEditMode;

        private string playlistTitleEdit;
        private string playlistAuthorEdit;
        private string playlistDescriptionEdit;

        private PlaylistSongDetailViewModel playlistSongDetail;
        private readonly BeatMapService beatMapService;

        private string sortColumnName;
        private SortingState sortingState;

        #endregion

        #region Public Properties

        /// <summary>
        /// Contains all songs of a playlist.
        /// </summary>
        public ObservableCollection<PlaylistSongViewModel> Songs { get; }

        /// <summary>
        /// Gets or sets the selected song.
        /// </summary>
        public PlaylistSongViewModel SelectedPlaylistSong
        {
            get => playlistSong;
            set
            {
                if (playlistSong == value) return;
                playlistSong = value;
                OnPropertyChanged();
                SongChanged(playlistSong);
            }
        }

        /// <summary>
        /// Gets or sets the viewmodel for the detail area.
        /// </summary>
        public PlaylistSongDetailViewModel PlaylistSongDetail
        {
            get => playlistSongDetail;
            set
            {
                if (value == playlistSongDetail) return;
                playlistSongDetail = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasPlaylistSongDetail));
            }
        }

        /// <summary>
        /// Gets whether a playlist song detail is available.
        /// </summary>
        public bool HasPlaylistSongDetail
        {
            get => playlistSongDetail != null;
        }

        /// <summary>
        /// Gets or sets the title of the playlist.
        /// </summary>
        public string PlaylistTitle
        {
            get => playlist.PlaylistTitle;
            set
            {
                if (value == playlist.PlaylistTitle) return;
                playlist.PlaylistTitle = value;
                OnPropertyChanged();
                Name = playlist.PlaylistTitle;
            }
        }

        /// <summary>
        /// Gets or sets the title of the playlist while in edit mode.
        /// </summary>
        public string PlaylistTitleEdit
        {
            get => playlistTitleEdit;
            set
            {
                if (value == playlistTitleEdit) return;
                playlistTitleEdit = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the cover image of the playlist.
        /// </summary>
        public ImageSource CoverImage
        {
            get
            {
                if (string.IsNullOrWhiteSpace(playlist.Image)) return null;
                return CSMImageConverter.BitmapFromBase64(playlist.Image.Split(',').Last());
            }
        }

        /// <summary>
        /// Gets or sets the author of the playlist.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the author of the playlist while in edit mode.
        /// </summary>
        public string PlaylistAuthorEdit
        {
            get => playlistAuthorEdit;
            set
            {
                if (value == playlistAuthorEdit) return;
                playlistAuthorEdit = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the description of the playlist.
        /// </summary>
        public string PlaylistDescription
        {
            get => playlist.PlaylistDescription;
            set
            {
                if (value == playlist.PlaylistDescription) return;
                playlist.PlaylistDescription = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the description of the playlist while in edit mode.
        /// </summary>
        public string PlaylistDescriptionEdit
        {
            get => playlistDescriptionEdit;
            set
            {
                if (value == playlistDescriptionEdit) return;
                playlistDescriptionEdit = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets whether the playlist has a sync url (eg. hitbloq).
        /// </summary>
        public bool HasSyncUrl
        {
            get => playlist.CustomData != null && playlist.CustomData.SyncURL != null;
        }

        public string SyncInfo
        {
            get
            {
                if (playlist.CustomData == null) return string.Empty;

                if (playlist.CustomData.SyncURL != null && playlist.CustomData.SyncURL.Contains("hitbloq"))
                {
                    return Resources.Workspace_Playlist_Hitbloq;
                }
                else if (playlist.CustomData.SyncURL != null && playlist.CustomData.SyncURL.Contains("accsaber"))
                {
                    return Resources.Workspace_Playlist_AccSaber;
                }
                else if (playlist.CustomData.SyncURL != null && playlist.CustomData.SyncURL.Contains("beatleader"))
                {
                    return Resources.Workspace_Playlist_BeatLeader;
                }
                else return string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets whether the playlist information is in edit mode.
        /// </summary>
        public bool InEditMode
        {
            get => inEditMode;
            set
            {
                if (value == inEditMode) return;
                inEditMode = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Command used to set the playlist information to edit mode.
        /// </summary>
        public RelayCommand EditCommand { get; }

        /// <summary>
        /// Command used to save the changes made while in edit mode.
        /// </summary>
        public RelayCommand SaveCommand { get; }

        /// <summary>
        /// Command used to reject the changes made while in edit mode.
        /// </summary>
        public RelayCommand CancelCommand { get; }

        /// <summary>
        /// Command used to save the playlist with the current song order.
        /// </summary>
        public RelayCommand SavePlaylistCommand { get; }

        /// <summary>
        /// Command used to fetch all song data for the current playlist.
        /// </summary>
        public AsyncRelayCommand FetchPlaylistDataCommand { get; }

        /// <summary>
        /// Command used to choose a new cover image.
        /// </summary>
        public RelayCommand ChooseCoverImageCommand { get; }

        #endregion

        /// <summary>
        /// Initializes a new <see cref="PlaylistViewModel"/>.
        /// </summary>
        /// <param name="playlist">The <see cref="Playlist"/>.</param>
        public PlaylistViewModel(Playlist playlist) : base(playlist.PlaylistTitle, playlist.Path)
        {
            this.playlist = playlist;

            EditCommand = new RelayCommand(Edit);
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
            SavePlaylistCommand = new RelayCommand(SavePlaylist);
            FetchPlaylistDataCommand = new AsyncRelayCommand(FetchPlaylistDataAsync, CanFetchPlaylistData);
            ChooseCoverImageCommand = new RelayCommand(ChooseCoverImage);

            InEditMode = false;

            beatMapService = new BeatMapService("maps/hash");

            Songs = new ObservableCollection<PlaylistSongViewModel>();
            foreach (var song in playlist.Songs)
            {
                var songViewModel = new PlaylistSongViewModel(song, playlist);
                songViewModel.DeleteSongEvent += SongViewModel_DeleteSongEvent;
                Songs.Add(songViewModel);
            }
        }

        /// <summary>
        /// Adds a song to the playlist.
        /// </summary>
        /// <param name="e">EventArgs containing the song informations.</param>
        public void AddPlaylistSong(AddSongToPlaylistEventArgs e)
        {
            var existingSong = playlist.Songs.SingleOrDefault(s => s.Hash == e.Hash);
            if (existingSong != null) return;

            var playlistSong = new PlaylistSong
            {
                Hash = e.Hash,
                Key = e.BsrKey,
                LevelAuthorName = e.LevelAuthorName,
                Levelid = e.LevelId,
                SongName = e.SongName
            };
            if (playlistSong == null) playlist.Songs = new List<PlaylistSong>();
            playlist.Songs.Add(playlistSong);

            SaveToFile();

            var songViewModel = new PlaylistSongViewModel(playlistSong, playlist);
            songViewModel.DeleteSongEvent += SongViewModel_DeleteSongEvent;
            Songs.Add(songViewModel);
        }

        /// <summary>
        /// Loads the custom level data from BeatSaver.
        /// </summary>
        /// <param name="hash">The hash of the beat map.</param>
        /// <returns>An awaitable task that yields no result.</returns>
        public async Task SetBeatSaverBeatMapDataAsync(string hash)
        {
            var beatmap = await beatMapService.GetBeatMapDataAsync(hash);
            if (beatmap == null)
            {
                PlaylistSongDetail = null;
            }
            else
            {
                PlaylistSongDetail = new PlaylistSongDetailViewModel(beatmap);
                if (SelectedPlaylistSong != null && (string.IsNullOrWhiteSpace(SelectedPlaylistSong.BsrKey) || string.IsNullOrWhiteSpace(SelectedPlaylistSong.SongName) || string.IsNullOrWhiteSpace(SelectedPlaylistSong.LevelAuthorName)))
                {
                    SelectedPlaylistSong.BsrKey = beatmap.Id;
                    SelectedPlaylistSong.SongName = beatmap.Metadata.SongName;
                    SelectedPlaylistSong.LevelAuthorName = beatmap.Metadata.SongAuthorName;
                    SaveToFile();
                }
            }
        }

        /// <summary>
        /// Checks if the playlist contains a song with the given hash.
        /// </summary>
        /// <param name="leftHash">Hash of the song.</param>
        /// <returns>True if the playlist contains the song; otherwise false.</returns>
        public override bool CheckContainsLeftSong(string leftHash)
        {
            if (string.IsNullOrWhiteSpace(leftHash)) return false;
            ContainsLeftSong = Songs.Any(s => s.Hash == leftHash);
            return ContainsLeftSong;
        }

        public override bool CheckContainsRightSong(string rightHash)
        {
            if (string.IsNullOrWhiteSpace(rightHash)) return false;
            ContainsRightSong = Songs.Any(s => s.Hash == rightHash);
            return ContainsRightSong;
        }

        public void SetSortOrder(string sortColumnName, Telerik.Windows.Controls.SortingState sortingState)
        {
            this.sortColumnName = sortColumnName;
            this.sortingState = sortingState;
        }

        #region Helper methods

        private void SongViewModel_DeleteSongEvent(object sender, System.EventArgs e)
        {
            var songViewModel = sender as PlaylistSongViewModel;
            songViewModel.DeleteSongEvent -= SongViewModel_DeleteSongEvent;
            Songs.Remove(songViewModel);

            SaveToFile();
        }

        private void Edit()
        {
            PlaylistTitleEdit = PlaylistTitle;
            PlaylistAuthorEdit = PlaylistAuthor;
            PlaylistDescriptionEdit = PlaylistDescription;
            InEditMode = true;
        }

        private void Save()
        {
            InEditMode = false;
            PlaylistTitle = PlaylistTitleEdit;
            PlaylistAuthor = PlaylistAuthorEdit;
            PlaylistDescription = PlaylistDescriptionEdit;

            SaveToFile();
        }

        private void Cancel()
        {
            PlaylistTitleEdit = string.Empty;
            PlaylistAuthorEdit = string.Empty;
            playlistDescriptionEdit = string.Empty;
            InEditMode = false;
        }

        private void SaveToFile()
        {
            // Save to file
            var options = new JsonSerializerOptions { WriteIndented = true };
            var content = JsonSerializer.Serialize(playlist, options);
            File.WriteAllText(playlist.Path, content);
        }

        private void SavePlaylist()
        {
            if (string.IsNullOrWhiteSpace(sortColumnName)) return;
            if (sortingState == Telerik.Windows.Controls.SortingState.None) return;
            var currentSongs = playlist.Songs.ToList();
            playlist.Songs.Clear();
            switch (sortColumnName)
            {
                case "BsrKeyHex":
                    if (sortingState == Telerik.Windows.Controls.SortingState.Ascending)
                    {
                        playlist.Songs.AddRange(currentSongs.OrderBy(s => s.BsrKeyHex));
                    }
                    else
                    {
                        playlist.Songs.AddRange(currentSongs.OrderByDescending(s => s.BsrKeyHex));
                    }
                    break;
                case "SongName":
                    if (sortingState == Telerik.Windows.Controls.SortingState.Ascending)
                    {
                        playlist.Songs.AddRange(currentSongs.OrderBy(s => s.SongName));
                    }
                    else
                    {
                        playlist.Songs.AddRange(currentSongs.OrderByDescending(s => s.SongName));
                    }
                    break;
                case "LevelAuthorName":
                    if (sortingState == Telerik.Windows.Controls.SortingState.Ascending)
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
            SaveToFile();
        }

        private async Task FetchPlaylistDataAsync()
        {
            try
            {
                int totalPages = Songs.Count / 50 + 1;
                for (int i = 0; i < totalPages; i++)
                {
                    var songHashes = Songs.Skip(i * 50).Take(50).Select(s => s.Hash);
                    var hashString = string.Join(",", songHashes);
                    var beatmaps = await beatMapService.GetBeatMapDatasAsync(hashString);
                    foreach (var keyValuePair in beatmaps)
                    {
                        if (keyValuePair.Value != null)
                        {
                            var existingSongs = Songs.Where(s => s.Hash == keyValuePair.Key);
                            foreach (var existingSong in existingSongs)
                            {
                                existingSong.BsrKey = keyValuePair.Value.Id;
                                existingSong.SongName = keyValuePair.Value.Metadata.SongName;
                                existingSong.LevelAuthorName = keyValuePair.Value.Metadata.LevelAuthorName;
                            }
                        }
                    }
                    SaveToFile();
                }
            }
            catch (System.Exception ex)
            {
                LoggerProvider.Logger.Error<PlaylistViewModel>($"Error while fetching song data for playlist '{PlaylistTitle}'. {ex.Message}");
            }
        }

        private bool CanFetchPlaylistData()
        {
            return true;
        }

        private void ChooseCoverImage()
        {
            var playlistPath = Path.Combine(UserConfigManager.Instance.Config.PlaylistPaths.First().Path, "CoverImages");
            if (!Directory.Exists(playlistPath)) playlistPath = "C:\\";

            RadOpenFileDialog openFileDialog = new RadOpenFileDialog();
            openFileDialog.Owner = AppCurrent.Current.MainWindow;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.InitialDirectory = playlistPath;
            openFileDialog.Filter = "|Image Files|*.jpg;*.png";
            openFileDialog.ShowDialog();
            if (openFileDialog.DialogResult == true)
            {
                playlist.Image = CSMImageConverter.StringFromBitmap(openFileDialog.FileName);
                SaveToFile();
                OnPropertyChanged(nameof(CoverImage));
            }
        }

        #endregion
    }
}