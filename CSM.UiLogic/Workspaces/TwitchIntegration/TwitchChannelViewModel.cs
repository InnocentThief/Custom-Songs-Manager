using CSM.Business.TwitchIntegration;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;

namespace CSM.UiLogic.Workspaces.TwitchIntegration
{
    /// <summary>
    /// Represents one configured twitch channel.
    /// </summary>
    public class TwitchChannelViewModel : ObservableObject
    {
        private string channelName;

        public string ChannelName
        {
            get => channelName;
            set
            {
                if (value == channelName) return;
                channelName = value;
                OnPropertyChanged();
            }
        }

        public bool CanEditName => !TwitchChannelManager.Instance.CheckChannelIsJoined(ChannelName);

        public RelayCommand JoinLeaveChannelCommand { get; }

        public bool Joined => TwitchChannelManager.Instance.CheckChannelIsJoined(ChannelName);

        public TwitchChannelViewModel()
        {
            JoinLeaveChannelCommand = new RelayCommand(JoinLeaveChannel);
            TwitchChannelManager.OnJoinedChannel += this.TwitchChannelManager_OnJoinedChannel;
            TwitchChannelManager.OnLeftChannel += TwitchChannelManager_OnLeftChannel;
            //TwitchChannelManager.Instance.AddChannel(Key, "InnocentThief");

        }

        private void TwitchChannelManager_OnLeftChannel(object sender, OnLeftChannelArgs e)
        {
            OnPropertyChanged(nameof(CanEditName));
            OnPropertyChanged(nameof(Joined));
        }

        private void TwitchChannelManager_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            OnPropertyChanged(nameof(CanEditName));
            OnPropertyChanged(nameof(Joined));
        }

        private void JoinLeaveChannel()
        {
            if (Joined)
            {
                TwitchChannelManager.Instance.LeaveChannel(ChannelName);
            }
            else
            {
                TwitchChannelManager.Instance.JoinChannel(ChannelName);
            }
        }
    }
}