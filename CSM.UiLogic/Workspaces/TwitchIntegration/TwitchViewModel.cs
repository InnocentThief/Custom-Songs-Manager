using CSM.Business.TwitchIntegration;
using CSM.Business.TwitchIntegration.TwitchConfiguration;
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

        public ObservableCollection<string> ReceivedBeatMaps { get; }

        #endregion

        public TwitchViewModel()
        {
            Channels = new ObservableCollection<TwitchChannelViewModel>();
            ReceivedBeatMaps = new ObservableCollection<string>();

            beatMapService = new BeatMapService("maps/id");

            AddChannelCommand = new RelayCommand(AddChannel);
            RemoveChannelCommand = new RelayCommand(RemoveChannel, CanRemoveChannel);
            TwitchChannelManager.OnBsrKeyReceived += TwitchChannelManager_OnBsrKeyReceived; ;
        }

        private async void TwitchChannelManager_OnBsrKeyReceived(object sender, string e)
        {
            var beatmap = await beatMapService.GetBeatMapDataAsync(e);
            if (beatmap == null) return;
            ReceivedBeatMaps.Add(beatmap.Name);
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
        }

        #region Helper methods

        private void AddChannel()
        {
            var newChannel = new TwitchChannelViewModel();
            Channels.Add(newChannel);
        }

        private void RemoveChannel()
        {
            Channels.Remove(selectedChannel);
        }

        private bool CanRemoveChannel()
        {
            return selectedChannel != null;
        }

        #endregion
    }
}