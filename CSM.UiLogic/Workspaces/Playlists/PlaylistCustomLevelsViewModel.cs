using CSM.DataAccess.Entities.Offline;
using CSM.Framework.Configuration.UserConfiguration;
using CSM.Framework.Logging;
using CSM.Services;
using CSM.UiLogic.Workspaces.CustomLevels;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.Playlists
{
    public class PlaylistCustomLevelsViewModel : ObservableObject
    {
        #region Private fields

        private CustomLevelViewModel selectedCustomLevel;
        private BackgroundWorker bgWorker;
        private bool isLoading;
        private int loadProgress;
        private PlaylistSongDetailViewModel playlistSongDetail;
        private BeatMapService beatMapService;
        private PlaylistSelectionState playlistSelectionState;

        #endregion

        #region Public Properties

        public ObservableCollection<CustomLevelViewModel> CustomLevels { get; }

        public CustomLevelViewModel SelectedCustomLevel
        {
            get => selectedCustomLevel;
            set
            {
                if (value == selectedCustomLevel) return;
                selectedCustomLevel = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<FavoriteViewModel> Favorites { get; }

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
        /// Gets or sets whether the data is loading.
        /// </summary>
        public bool IsLoading
        {
            get => isLoading;
            set
            {
                if (value == isLoading) return;
                isLoading = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the load progress.
        /// </summary>
        public int LoadProgress
        {
            get => loadProgress;
            set
            {
                loadProgress = (int)(100.0 / 2145 * value);
                OnPropertyChanged();
            }
        }

        #endregion

        public event EventHandler<AddSongToPlaylistEventArgs> AddSongToPlaylistEvent;

        public PlaylistCustomLevelsViewModel(PlaylistSelectionState playlistSelectionState)
        {
            this.playlistSelectionState = playlistSelectionState;
            playlistSelectionState.PlaylistSelectionChangedEvent += PlaylistSelectionState_PlaylistSelectionChangedEvent;

            CustomLevels = new ObservableCollection<CustomLevelViewModel>();
            Favorites = new ObservableCollection<FavoriteViewModel>();
            beatMapService = new BeatMapService("maps/id");
        }

        public async Task GetBeatSaverBeatMapDataAsync(string key)
        {
            var beatmap = await beatMapService.GetBeatMapDataAsync(key);
            PlaylistSongDetail = beatmap == null ? null : new PlaylistSongDetailViewModel(beatmap);
        }

        public void LoadData()
        {
            CustomLevels.Clear();

            bgWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            bgWorker.DoWork += BackgroundWorker_DoWork;
            bgWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            bgWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            bgWorker.RunWorkerAsync();
        }

        public async Task LoadFavoritesAsync()
        {
            var locallow = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace("Roaming", "LocalLow");
            var playerDataFile = Path.Combine(locallow, "Hyperbolic Magnetism\\Beat Saber\\PlayerData.dat");
            var playerDataContent = File.ReadAllText(playerDataFile);
            var playerData = JsonSerializer.Deserialize<PlayerData>(playerDataContent);

            var favoriteBeatMapService = new BeatMapService("maps/hash");

            foreach (var favorite_levelId in playerData.LocalPlayers.First().FavoritesLevelIds)
            {
                var hash = favorite_levelId.Substring(13);
                var beatmap = await favoriteBeatMapService.GetBeatMapDataAsync(hash);
                if (beatmap == null) continue;
                var favoriteViewModel = new FavoriteViewModel(beatmap);
                Favorites.Add(favoriteViewModel);
            }
        }

        #region Helper methods

        private void PlaylistSelectionState_PlaylistSelectionChangedEvent(object sender, EventArgs e)
        {
            foreach (var customLevel in CustomLevels)
            {
                customLevel.SetCanAddToPlaylist(playlistSelectionState.PlaylistSelected);
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                IsLoading = true;

                var i = 0;
                var levels = new List<CustomLevel>();
                var customLevelPath = UserConfigManager.Instance.Config.CustomLevelPaths.First().Path;


                if (!Directory.Exists(customLevelPath)) return;

                IEnumerable<string> folderEntries = Directory.EnumerateDirectories(customLevelPath);
                foreach (string folderEntry in folderEntries)
                {
                    if (bgWorker.CancellationPending) return;
                    var info = Path.Combine(folderEntry, "Info.dat");
                    if (File.Exists(info))
                    {
                        var infoContent = File.ReadAllText(info);
                        CustomLevel customLevel = JsonSerializer.Deserialize<CustomLevel>(infoContent);
                        if (customLevel != null)
                        {
                            var directory = new DirectoryInfo(folderEntry);
                            try
                            {
                                customLevel.BsrKey = directory.Name.Substring(0, directory.Name.IndexOf(" "));
                                customLevel.ChangeDate = File.GetLastWriteTime(info);
                                customLevel.Path = folderEntry;
                            }
                            catch (Exception)
                            {
                                LoggerProvider.Logger.Info<CustomLevelsViewModel>($"Unable to get key for {directory.FullName}. Wrong directory name");
                            }
                            levels.Add(customLevel);
                        }
                    }
                    i++;
                    bgWorker.ReportProgress(i);
                }
                e.Result = levels;
            }
            catch (Exception ex)
            {
                LoggerProvider.Logger.Error<CustomLevelsViewModel>($"Unable to load custom levels: {ex}");
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var customLevels = (List<CustomLevel>)e.Result;
            if (customLevels != null)
            {
                foreach (var customLevel in customLevels)
                {
                    var customLevelViewModel = new CustomLevelViewModel(customLevel);
                    customLevelViewModel.AddSongToPlaylistEvent += CustomLevelViewModel_AddSongToPlaylistEvent;
                    CustomLevels.Add(customLevelViewModel);
                }
            }
            IsLoading = false;

            if (bgWorker != null)
            {
                bgWorker.DoWork -= BackgroundWorker_DoWork;
                bgWorker.ProgressChanged -= BackgroundWorker_ProgressChanged;
                bgWorker.RunWorkerCompleted -= BackgroundWorker_RunWorkerCompleted;
                bgWorker.Dispose();
                bgWorker = null;
            }
        }

        private async void CustomLevelViewModel_AddSongToPlaylistEvent(object sender, AddSongToPlaylistEventArgs e)
        {
            var beatmap = await beatMapService.GetBeatMapDataAsync(e.BsrKey);
            e.Hash = beatmap.Versions.First().Hash;
            e.SongName = beatmap.Metadata.SongName;
            e.LevelAuthorName = beatmap.Metadata.LevelAuthorName;
            e.LevelId = $"custom_level_{e.Hash}";

            AddSongToPlaylistEvent?.Invoke(this, e);
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            LoadProgress = e.ProgressPercentage;
        }

        #endregion
    }
}