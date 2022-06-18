using CSM.Business.TwitchIntegration;
using CSM.Business.TwitchIntegration.TwitchConfiguration;
using CSM.DataAccess.Entities.Offline;
using CSM.Services;
using CSM.UiLogic.Workspaces.Playlists;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.TwitchIntegration
{
    public class TwitchViewModel : ObservableObject
    {
        #region Private fields

        private TwitchChannelViewModel selectedChannel;
        private BeatMapService beatMapService;
        private ReceivedBeatmapViewModel selectedBeatmap;
        private bool initialized;
        private PlaylistSelectionState playlistSelectionState;
        private PlaylistSongDetailViewModel playlistSongDetail;

        #endregion

        #region Public Properties

        public string AuthenticatedAs
        {
            get
            {
                if (string.IsNullOrWhiteSpace(TwitchConfigManager.Instance.Config.Login))
                {
                    return "Twitch: not logged in";
                }
                else
                {
                    return $"Twitch: logged in as {TwitchConfigManager.Instance.Config.Login}";
                }
            }
        }

        public RelayCommand AddChannelCommand { get; }

        public RelayCommand RemoveChannelCommand { get; }

        public ObservableCollection<TwitchChannelViewModel> Channels { get; }

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

        public ObservableCollection<ReceivedBeatmapViewModel> ReceivedBeatmaps { get; }

        public ReceivedBeatmapViewModel SelectedBeatmap
        {
            get => selectedBeatmap;
            set
            {
                if (value == selectedBeatmap) return;
                selectedBeatmap = value;
                OnPropertyChanged();
                ClearReceivedBeatmapsCommand.NotifyCanExecuteChanged();
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

        #endregion

        /// <summary>
        /// Occurs when a selected song changes.
        /// </summary>
        public event EventHandler<PlaylistSongChangedEventArgs> SongChangedEvent;

        public TwitchViewModel(PlaylistSelectionState playlistSelectionState)
        {
            this.playlistSelectionState = playlistSelectionState;
            playlistSelectionState.PlaylistSelectionChangedEvent += PlaylistSelectionState_PlaylistSelectionChangedEvent; ;

            Channels = new ObservableCollection<TwitchChannelViewModel>();
            ReceivedBeatmaps = new ObservableCollection<ReceivedBeatmapViewModel>();

            beatMapService = new BeatMapService("maps/id");

            AddChannelCommand = new RelayCommand(AddChannel);
            RemoveChannelCommand = new RelayCommand(RemoveChannel, CanRemoveChannel);
            ClearReceivedBeatmapsCommand = new RelayCommand(ClearReceivedBeatmaps, CanClearReceivedBeatmaps);

            TwitchChannelManager.OnBsrKeyReceived += TwitchChannelManager_OnBsrKeyReceived; ;
        }

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

            foreach (var channel in TwitchConfigManager.Instance.Config.Channels)
            {
                var twitchChannelViewModel = new TwitchChannelViewModel { Name = channel.Name };
                twitchChannelViewModel.ChangedConnectionState += TwitchChannelViewModel_ChangedConnectionState;
                Channels.Add(twitchChannelViewModel);
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
            ReceivedBeatmaps.Add(receivedBeatmapViewModel);
            //ClearReceivedBeatmapsCommand.NotifyCanExecuteChanged();
        }

        private void AddChannel()
        {
            var newChannel = new TwitchChannelViewModel();
            newChannel.ChangedConnectionState += TwitchChannelViewModel_ChangedConnectionState;
            Channels.Add(newChannel);
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
            foreach (var receivedBeatmap in ReceivedBeatmaps)
            {
                receivedBeatmap.AddSongToPlaylistEvent -= ReceivedBeatmapViewModel_AddSongToPlaylistEvent;
            }
            ReceivedBeatmaps.Clear();
            PlaylistSongDetail = null;
        }

        private bool CanClearReceivedBeatmaps()
        {
            return ReceivedBeatmaps.Any();
        }

        private void PlaylistSelectionState_PlaylistSelectionChangedEvent(object sender, EventArgs e)
        {
            foreach (var receivedBeatmap in ReceivedBeatmaps)
            {
                receivedBeatmap.SetCanAddToPlaylist(playlistSelectionState.PlaylistSelected);
            }
        }

        private void ReceivedBeatmapViewModel_AddSongToPlaylistEvent(object sender, AddSongToPlaylistEventArgs e)
        {
            AddSongToPlaylistEvent?.Invoke(this, e);
        }

        #endregion
    }
}