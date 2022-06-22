using CSM.Business.TwitchIntegration;
using CSM.Business.TwitchIntegration.TwitchConfiguration;
using CSM.DataAccess.Entities.Offline;
using CSM.Framework.Configuration.UserConfiguration;
using CSM.Services;
using CSM.UiLogic.Properties;
using CSM.UiLogic.Workspaces.Playlists;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.TwitchIntegration
{
    /// <summary>
    /// ViewModel for Twitch related stuff.
    /// </summary>
    public class TwitchViewModel : ObservableObject
    {
        #region Private fields

        private TwitchChannelViewModel selectedChannel;
        private BeatMapService beatMapService;
        private ReceivedBeatmapViewModel selectedBeatmap;
        private bool initialized;
        private PlaylistSelectionState playlistSelectionState;
        private PlaylistSongDetailViewModel playlistSongDetail;
        private List<AddSongToPlaylistEventArgs> autoAddSongs;
        private string autoAddText = Resources.TwitchIntegration_AutoAddingNotActive;
        private string autoAddFilePath;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the user currently authenticated with Twitch.
        /// </summary>
        public string AuthenticatedAs
        {
            get
            {
                if (string.IsNullOrWhiteSpace(TwitchConfigManager.Instance.Config.Login))
                {
                    return Resources.TwitchIntegration_NotLoggedIn;
                }
                else
                {
                    return string.Format(Resources.TwitchIntegration_LoggedIn, TwitchConfigManager.Instance.Config.Login);
                }
            }
        }

        /// <summary>
        /// Command used to add a new channel.
        /// </summary>
        public AsyncRelayCommand AddChannelCommand { get; }

        /// <summary>
        /// Command used to remove a channel.
        /// </summary>
        public RelayCommand RemoveChannelCommand { get; }

        /// <summary>
        /// Contains the configured channels.
        /// </summary>
        public ObservableCollection<TwitchChannelViewModel> Channels { get; }

        /// <summary>
        /// Gets or sets the selected channel.
        /// </summary>
        public TwitchChannelViewModel SelectedChannel
        {
            get => selectedChannel;
            set
            {
                if (value == selectedChannel) return;
                selectedChannel = value;
                OnPropertyChanged();
                RemoveChannelCommand.NotifyCanExecuteChanged();
            }
        }

        /// <summary>
        /// Contains the received beatmaps.
        /// </summary>
        public ObservableCollection<ReceivedBeatmapViewModel> ReceivedBeatmaps { get; }

        /// <summary>
        /// Gets or sets the selected beatmap.
        /// </summary>
        public ReceivedBeatmapViewModel SelectedBeatmap
        {
            get => selectedBeatmap;
            set
            {
                if (value == selectedBeatmap) return;
                selectedBeatmap = value;
                OnPropertyChanged();
                if (selectedBeatmap != null)
                {
                    SongChangedEvent?.Invoke(this, new PlaylistSongChangedEventArgs { LeftHash = selectedBeatmap.Hash });
                }
                else
                {
                    SongChangedEvent?.Invoke(this, new PlaylistSongChangedEventArgs { LeftHash = String.Empty });
                }
            }
        }

        /// <summary>
        /// Occurs on adding a song to a playlist.
        /// </summary>
        public event EventHandler<AddSongToPlaylistEventArgs> AddSongToPlaylistEvent;

        public RelayCommand ClearReceivedBeatmapsCommand { get; }

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
        /// Command used to start the auto-add function.
        /// </summary>
        public RelayCommand StartAutoAddCommand { get; }

        /// <summary>
        /// Command used to stop the auto-add function.
        /// </summary>
        public RelayCommand StopAutoAddCommand { get; }

        public string AutoAddText
        {
            get => autoAddText;
            set
            {
                if (autoAddText == value) return;
                autoAddText = value;
                OnPropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// Occurs when a selected song changes.
        /// </summary>
        public event EventHandler<PlaylistSongChangedEventArgs> SongChangedEvent;

        /// <summary>
        /// Initializes a new <see cref="TwitchViewModel"/>.
        /// </summary>
        /// <param name="playlistSelectionState">Contains the playlist state.</param>
        public TwitchViewModel(PlaylistSelectionState playlistSelectionState)
        {
            this.playlistSelectionState = playlistSelectionState;
            playlistSelectionState.PlaylistSelectionChangedEvent += PlaylistSelectionState_PlaylistSelectionChangedEvent; ;

            Channels = new ObservableCollection<TwitchChannelViewModel>();
            ReceivedBeatmaps = new ObservableCollection<ReceivedBeatmapViewModel>();

            beatMapService = new BeatMapService("maps/id");

            AddChannelCommand = new AsyncRelayCommand(AddChannelAsync);
            RemoveChannelCommand = new RelayCommand(RemoveChannel, CanRemoveChannel);
            ClearReceivedBeatmapsCommand = new RelayCommand(ClearReceivedBeatmaps);

            StartAutoAddCommand = new RelayCommand(StartAutoAdd, CanStartAutoAdd);
            StopAutoAddCommand = new RelayCommand(StopAutoAdd, CanStopAutoAdd);

            TwitchChannelManager.OnBsrKeyReceived += TwitchChannelManager_OnBsrKeyReceived;
        }

        /// <summary>
        /// Initializes the data for Twitch view. 
        /// </summary>
        public async void Initialize()
        {
            var valid = await TwitchConnectionManager.Instance.ValidateAsync();
            if (!valid)
            {
                await TwitchConnectionManager.Instance.GetAccessTokenAsync();
                await TwitchConnectionManager.Instance.ValidateAsync();
            }
            OnPropertyChanged(nameof(AuthenticatedAs));

            if (initialized) return;

            // Get configured channels
            foreach (var channel in TwitchConfigManager.Instance.Config.Channels)
            {
                var twitchChannelViewModel = new TwitchChannelViewModel { Name = channel.Name };
                twitchChannelViewModel.ChangedConnectionState += TwitchChannelViewModel_ChangedConnectionState;
                Channels.Add(twitchChannelViewModel);
            }

            // Get the saved beatmaps
            foreach (var receivedBeatmap in ReceivedBeatmapsManager.Instance.ReceivedBeatmaps.Beatmaps)
            {
                var receivedBeatmapViewModel = new ReceivedBeatmapViewModel(receivedBeatmap);
                receivedBeatmapViewModel.SetCanAddToPlaylist(playlistSelectionState.PlaylistSelected);
                receivedBeatmapViewModel.AddSongToPlaylistEvent += ReceivedBeatmapViewModel_AddSongToPlaylistEvent;
                receivedBeatmapViewModel.DeleteSongEvent += ReceivedBeatmapViewModel_DeleteSongEvent;
                ReceivedBeatmaps.Add(receivedBeatmapViewModel);
            }

            initialized = true;
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

        #region Helper methods

        private async void TwitchChannelManager_OnBsrKeyReceived(object sender, SongRequestEventArgs e)
        {
            var beatmap = await beatMapService.GetBeatMapDataAsync(e.Key);
            if (beatmap == null) return;

            if (autoAddSongs == null)
            {
                var receivedBeatmap = new ReceivedBeatmap()
                {
                    ChannelName = e.ChannelName,
                    ReceivedAt = DateTime.Now,
                    Key = beatmap.Id,
                    Hash = beatmap.LatestVersion.Hash,
                    SongName = beatmap.Metadata.SongName,
                    LevelAuthorName = beatmap.Metadata.LevelAuthorName,
                    SongAuthorName = beatmap.Metadata.SongAuthorName
                };
                var receivedBeatmapViewModel = new ReceivedBeatmapViewModel(receivedBeatmap);
                receivedBeatmapViewModel.SetCanAddToPlaylist(playlistSelectionState.PlaylistSelected);
                receivedBeatmapViewModel.AddSongToPlaylistEvent += ReceivedBeatmapViewModel_AddSongToPlaylistEvent;
                receivedBeatmapViewModel.DeleteSongEvent += ReceivedBeatmapViewModel_DeleteSongEvent;
                ReceivedBeatmaps.Add(receivedBeatmapViewModel);
                ReceivedBeatmapsManager.Instance.AddBeatmap(receivedBeatmap);
            }
            else
            {
                var addSongEventArgs = new AddSongToPlaylistEventArgs
                {
                    BsrKey = beatmap.Id,
                    Hash = beatmap.LatestVersion.Hash,
                    LevelAuthorName = beatmap.Metadata.LevelAuthorName,
                    LevelId = $"custom_level_{beatmap.LatestVersion.Hash}",
                    SongName = beatmap.Metadata.SongName
                };
                autoAddSongs.Add(addSongEventArgs);
            }
        }

        private async Task AddChannelAsync()
        {
            await TwitchConnectionManager.Instance.ValidateAsync();
            var newChannel = new TwitchChannelViewModel();
            newChannel.ChangedConnectionState += TwitchChannelViewModel_ChangedConnectionState;
            Channels.Add(newChannel);
            OnPropertyChanged(nameof(AuthenticatedAs));
        }

        private void RemoveChannel()
        {
            TwitchConfigManager.Instance.RemoveChannel(selectedChannel.Name);
            selectedChannel.ChangedConnectionState -= TwitchChannelViewModel_ChangedConnectionState;
            Channels.Remove(selectedChannel);
        }

        private bool CanRemoveChannel()
        {
            return selectedChannel != null && !selectedChannel.Joined;
        }

        private void TwitchChannelViewModel_ChangedConnectionState(object sender, EventArgs e)
        {
            // invoke??
            //RemoveChannelCommand.NotifyCanExecuteChanged();
        }

        private void ClearReceivedBeatmaps()
        {
            ReceivedBeatmapsManager.Instance.RemoveBeatmaps(ReceivedBeatmaps.Select(rbm => rbm.ReceivedBeatmap).ToList());

            foreach (var receivedBeatmap in ReceivedBeatmaps)
            {
                receivedBeatmap.AddSongToPlaylistEvent -= ReceivedBeatmapViewModel_AddSongToPlaylistEvent;
            }
            ReceivedBeatmaps.Clear();
            PlaylistSongDetail = null;
        }

        private void PlaylistSelectionState_PlaylistSelectionChangedEvent(object sender, EventArgs e)
        {
            foreach (var receivedBeatmap in ReceivedBeatmaps)
            {
                receivedBeatmap.SetCanAddToPlaylist(playlistSelectionState.PlaylistSelected);
            }
            StartAutoAddCommand.NotifyCanExecuteChanged();
            StopAutoAddCommand.NotifyCanExecuteChanged();
        }

        private void ReceivedBeatmapViewModel_DeleteSongEvent(object sender, EventArgs e)
        {
            var receivedBeatmap = sender as ReceivedBeatmapViewModel;
            receivedBeatmap.DeleteSongEvent -= ReceivedBeatmapViewModel_DeleteSongEvent;
            receivedBeatmap.AddSongToPlaylistEvent -= ReceivedBeatmapViewModel_AddSongToPlaylistEvent;
            ReceivedBeatmapsManager.Instance.RemoveBeatmaps(new System.Collections.Generic.List<ReceivedBeatmap> { receivedBeatmap.ReceivedBeatmap });
            ReceivedBeatmaps.Remove(receivedBeatmap);
        }

        private void ReceivedBeatmapViewModel_AddSongToPlaylistEvent(object sender, AddSongToPlaylistEventArgs e)
        {
            AddSongToPlaylistEvent?.Invoke(this, e);
            if (UserConfigManager.Instance.Config.RemoveReceivedSongAfterAddingToPlaylist)
            {
                var songToRemove = ReceivedBeatmaps.SingleOrDefault(rbm => rbm.Key == e.BsrKey);
                if (songToRemove != null)
                {
                    songToRemove.AddSongToPlaylistEvent -= ReceivedBeatmapViewModel_AddSongToPlaylistEvent;
                    ReceivedBeatmaps.Remove(songToRemove);
                }
            }
        }

        #region AutoAdd

        private void StartAutoAdd()
        {
            autoAddSongs = new List<AddSongToPlaylistEventArgs>();
            AutoAddText = string.Format(Resources.TwitchIntegration_AutoAddingTo, playlistSelectionState.PlaylistViewModel.Name);
            autoAddFilePath = playlistSelectionState.PlaylistViewModel.FilePath;
            StartAutoAddCommand.NotifyCanExecuteChanged();
            StopAutoAddCommand.NotifyCanExecuteChanged();
        }

        private bool CanStartAutoAdd()
        {
            return playlistSelectionState.PlaylistViewModel != null && autoAddSongs == null;
        }

        private void StopAutoAdd()
        {
            AutoAddText = Resources.TwitchIntegration_AutoAddingNotActive;

            var infoContent = File.ReadAllText(autoAddFilePath);
            Playlist playlist = JsonSerializer.Deserialize<Playlist>(infoContent);
            playlist.Path = autoAddFilePath;
            var playlistViewModel = new PlaylistViewModel(playlist);

            foreach (var autoAddSong in autoAddSongs)
            {
                playlistViewModel.AddPlaylistSong(autoAddSong);
            }

            autoAddSongs.Clear();
            autoAddSongs = null;

            autoAddFilePath = String.Empty;

            StartAutoAddCommand.NotifyCanExecuteChanged();
            StopAutoAddCommand.NotifyCanExecuteChanged();
        }

        private bool CanStopAutoAdd()
        {
            return autoAddSongs != null;
        }

        #endregion

        #endregion
    }
}