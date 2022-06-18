using CSM.Business.TwitchIntegration;
using CSM.Business.TwitchIntegration.TwitchConfiguration;
using CSM.DataAccess.Entities.Offline;
using CSM.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.TwitchIntegration
{
    public class TwitchViewModel : ObservableObject
    {
        private TwitchChannelViewModel selectedChannel;
        private BeatMapService beatMapService;
        private ReceivedBeatmap selectedBeatmap;
        private bool initialized;

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

        public ObservableCollection<ReceivedBeatmap> ReceivedBeatmaps { get; }

        public ReceivedBeatmap SelectedBeatmap
        {
            get => selectedBeatmap;
            set
            {
                if (value == selectedBeatmap) return;
                selectedBeatmap = value;
                OnPropertyChanged();
                RemoveReceivedBeatmapCommand.NotifyCanExecuteChanged();
            }
        }

        public RelayCommand RemoveReceivedBeatmapCommand { get; }

        #endregion

        public TwitchViewModel()
        {
            Channels = new ObservableCollection<TwitchChannelViewModel>();
            ReceivedBeatmaps = new ObservableCollection<ReceivedBeatmap>();

            beatMapService = new BeatMapService("maps/id");

            AddChannelCommand = new RelayCommand(AddChannel);
            RemoveChannelCommand = new RelayCommand(RemoveChannel, CanRemoveChannel);
            RemoveReceivedBeatmapCommand = new RelayCommand(RemoveReceivedBeatmap, CanRemoveReceivedBeatmap);

            TwitchChannelManager.OnBsrKeyReceived += TwitchChannelManager_OnBsrKeyReceived; ;
        }

        private async void TwitchChannelManager_OnBsrKeyReceived(object sender, SongRequestEventArgs e)
        {
            var beatmap = await beatMapService.GetBeatMapDataAsync(e.Key);
            if (beatmap == null) return;
            var receivedBeatmap = new ReceivedBeatmap()
            {
                ChannelName = e.ChannelName,
                ReceivedAt = DateTime.Now,
                Key = beatmap.Id,
                SongName = beatmap.Metadata.SongName,
                LevelAuthorName = beatmap.Metadata.LevelAuthorName,
                SongAuthorName = beatmap.Metadata.SongAuthorName
            };
            ReceivedBeatmaps.Add(receivedBeatmap);
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

        #region Helper methods

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

        private void RemoveReceivedBeatmap()
        {

        }

        public bool CanRemoveReceivedBeatmap()
        {
            return selectedBeatmap != null;
        }

        #endregion
    }
}