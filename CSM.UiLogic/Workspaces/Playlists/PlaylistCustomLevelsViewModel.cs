using CSM.DataAccess.Entities.Offline;
using CSM.Framework.Configuration.UserConfiguration;
using CSM.Framework.Logging;
using CSM.Services;
using CSM.UiLogic.Workspaces.CustomLevels;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
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
    /// <summary>
    /// ViewModel used to handle custom levels and favorites on the playlist workspace.
    /// </summary>
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
        private FavoriteViewModel selectedFavorite;
        private SearchedSongViewModel selectedSearchedSong;

        #endregion

        #region Public Properties

        /// <summary>
        /// Contains all custom levels.
        /// </summary>
        public ObservableCollection<CustomLevelViewModel> CustomLevels { get; }

        /// <summary>
        /// Gets or sets the selected custom level.
        /// </summary>
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

        /// <summary>
        /// Contains all favorites.
        /// </summary>
        public ObservableCollection<FavoriteViewModel> Favorites { get; }

        /// <summary>
        /// Gets or sets the selected favorite.
        /// </summary>
        public FavoriteViewModel SelectedFavorite
        {
            get => selectedFavorite;
            set
            {
                if (value == selectedFavorite) return;
                selectedFavorite = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// ViewModel for song search.
        /// </summary>
        public SongSearchViewModel SongSearch { get; }

        /// <summary>
        /// Contains all search songs.
        /// </summary>
        public ObservableCollection<SearchedSongViewModel> SearchedSongs { get; }

        /// <summary>
        /// Gets or sets the selected searched song.
        /// </summary>
        public SearchedSongViewModel SelectedSearchedSong
        {
            get => selectedSearchedSong;
            set
            {
                if (value == selectedSearchedSong) return;
                selectedSearchedSong = value;
                OnPropertyChanged();
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
                SongChanged(playlistSongDetail);
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

        /// <summary>
        /// Gets or sets the selected tab.
        /// </summary>
        public int SelectedTabIndex { get; set; }

        /// <summary>
        /// Command used to refresh either custom levels or favorites.
        /// </summary>
        public AsyncRelayCommand RefreshCommand { get; }

        #endregion

        /// <summary>
        /// Occurs on adding a song to a playlist.
        /// </summary>
        public event EventHandler<AddSongToPlaylistEventArgs> AddSongToPlaylistEvent;

        public event EventHandler<PlaylistSongChangedEventArgs> SongChangedEvent;

        /// <summary>
        /// Initializes a new <see cref="PlaylistCustomLevelsViewModel"/>.
        /// </summary>
        /// <param name="playlistSelectionState">The current playlistSelectionState.</param>
        public PlaylistCustomLevelsViewModel(PlaylistSelectionState playlistSelectionState)
        {
            this.playlistSelectionState = playlistSelectionState;
            playlistSelectionState.PlaylistSelectionChangedEvent += PlaylistSelectionState_PlaylistSelectionChangedEvent;

            CustomLevels = new ObservableCollection<CustomLevelViewModel>();
            Favorites = new ObservableCollection<FavoriteViewModel>();
            SearchedSongs = new ObservableCollection<SearchedSongViewModel>();
            SongSearch = new SongSearchViewModel();
            SongSearch.SearchSongEvent += SongSearch_SearchSongEvent;
            beatMapService = new BeatMapService("maps/id");

            RefreshCommand = new AsyncRelayCommand(RefreshAsync);
        }

        /// <summary>
        /// Gets the BeatSaver data for the given key.
        /// </summary>
        /// <param name="key">BSR key of the beatmap.</param>
        /// <returns>An awaitable task that yields no result.</returns>
        public async Task GetBeatSaverBeatMapDataAsync(string key)
        {
            var beatmap = await beatMapService.GetBeatMapDataAsync(key);
            PlaylistSongDetail = beatmap == null ? null : new PlaylistSongDetailViewModel(beatmap);
        }

        public void ShowSongDetailForSelectedSong()
        {
            PlaylistSongDetail = new PlaylistSongDetailViewModel(SelectedSearchedSong.BeatMap);
        }

        /// <summary>
        /// Loads the custom level data.
        /// </summary>
        public void LoadData()
        {
            foreach (var customLevel in CustomLevels)
            {
                customLevel.AddSongToPlaylistEvent -= CustomLevelOrFavorite_AddSongToPlaylistEvent;
            }
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

        /// <summary>
        /// Loads the favorites in asynchronous fashion.
        /// </summary>
        /// <param name="forceRefresh">Indicates whether to force the loading.</param>
        /// <returns>An awaitable task that yields no result.</returns>
        public async Task LoadFavoritesAsync(bool forceRefresh)
        {
            if (!forceRefresh && Favorites.Any()) return;
            foreach (var favorite in Favorites)
            {
                favorite.AddSongToPlaylistEvent -= CustomLevelOrFavorite_AddSongToPlaylistEvent;
            }
            Favorites.Clear();

            var locallow = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace("Roaming", "LocalLow");
            var playerDataFile = Path.Combine(locallow, "Hyperbolic Magnetism\\Beat Saber\\PlayerData.dat");
            var playerDataContent = File.ReadAllText(playerDataFile);
            var playerData = JsonSerializer.Deserialize<PlayerData>(playerDataContent);

            var favoriteBeatMapService = new BeatMapService("maps/hash");

            foreach (var favorite_levelId in playerData.LocalPlayers.First().FavoritesLevelIds)
            {
                if (!favorite_levelId.StartsWith("custom_level")) continue;
                var hash = favorite_levelId.Substring(13);
                var beatmap = await favoriteBeatMapService.GetBeatMapDataAsync(hash);
                if (beatmap == null) continue;
                var favoriteViewModel = new FavoriteViewModel(beatmap);
                favoriteViewModel.AddSongToPlaylistEvent += CustomLevelOrFavorite_AddSongToPlaylistEvent;
                Favorites.Add(favoriteViewModel);
            }
        }

        #region Helper methods

        private async Task RefreshAsync()
        {
            if (SelectedTabIndex == 0)
            {
                LoadData();
            }
            else
            {
                await LoadFavoritesAsync(true);
            }
        }

        private void PlaylistSelectionState_PlaylistSelectionChangedEvent(object sender, EventArgs e)
        {
            foreach (var customLevel in CustomLevels)
            {
                customLevel.SetCanAddToPlaylist(playlistSelectionState.PlaylistSelected);
            }

            foreach (var favorite in Favorites)
            {
                favorite.SetCanAddToPlaylist(playlistSelectionState.PlaylistSelected);
            }

            foreach (var searchedSong in SearchedSongs)
            {
                searchedSong.SetCanAddToPlaylist(playlistSelectionState.PlaylistSelected);
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
                                customLevel.ChangeDate = Directory.GetLastWriteTime(folderEntry);
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
                    customLevelViewModel.AddSongToPlaylistEvent += CustomLevelOrFavorite_AddSongToPlaylistEvent;
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

        private async void CustomLevelOrFavorite_AddSongToPlaylistEvent(object sender, AddSongToPlaylistEventArgs e)
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

        private void SongChanged(PlaylistSongDetailViewModel playlistSongDetail)
        {
            if (playlistSongDetail == null) return;
            SongChangedEvent?.Invoke(this, new PlaylistSongChangedEventArgs() { RightHash = playlistSongDetail.Hash });
        }

        private async void SongSearch_SearchSongEvent(object sender, SongSearchEventArgs e)
        {
            foreach (var searchedSong in SearchedSongs)
            {
                searchedSong.AddSongToPlaylistEvent -= CustomLevelOrFavorite_AddSongToPlaylistEvent;
            }
            SearchedSongs.Clear();

            var searchService = new BeatMapService("search/text/0");
            var beatmaps = await searchService.SearchSongsAsync(e.SearchString);

            foreach (var beatmap in beatmaps.Docs)
            {
                var searchedSong = new SearchedSongViewModel(beatmap);
                searchedSong.AddSongToPlaylistEvent += CustomLevelOrFavorite_AddSongToPlaylistEvent;
                searchedSong.SetCanAddToPlaylist(playlistSelectionState.PlaylistSelected);
                SearchedSongs.Add(searchedSong);
            }

            SongSearch.SetSearchParametersVisibility(!SearchedSongs.Any());
        }

        #endregion
    }
}