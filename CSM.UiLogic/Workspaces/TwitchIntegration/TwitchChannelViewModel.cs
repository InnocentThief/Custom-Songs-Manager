using CSM.Business.TwitchIntegration;
using CSM.Business.TwitchIntegration.TwitchConfiguration;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using TwitchLib.Client.Events;

namespace CSM.UiLogic.Workspaces.TwitchIntegration
{
    /// <summary>
    /// Represents one configured twitch channel.
    /// </summary>
    public class TwitchChannelViewModel : ObservableObject
    {
        private string name;

        #region Public Properties

        public string Name
        {
            get => name;
            set
            {
                if (value == name) return;
                name = value;
                OnPropertyChanged();
            }
        }

        public bool CanEditName => !TwitchChannelManager.Instance.CheckChannelIsJoined(Name);

        public RelayCommand JoinLeaveChannelCommand { get; }

        public string JoinLeaveChannelToolTip
        {
            get
            {
                if (Joined) return "Leave channel";
                return "Join channel";
            }
        }

        public bool Joined => TwitchChannelManager.Instance.CheckChannelIsJoined(Name);

        #endregion

        public event EventHandler ChangedConnectionState;

        public TwitchChannelViewModel()
        {
            JoinLeaveChannelCommand = new RelayCommand(JoinLeaveChannel);
            TwitchChannelManager.OnJoinedChannel += this.TwitchChannelManager_OnJoinedChannel;
            TwitchChannelManager.OnLeftChannel += TwitchChannelManager_OnLeftChannel;
        }

        private void TwitchChannelManager_OnLeftChannel(object sender, OnLeftChannelArgs e)
        {
            if (e.Channel.Equals(Name, StringComparison.InvariantCultureIgnoreCase))
            {
                OnPropertyChanged(nameof(CanEditName));
                OnPropertyChanged(nameof(Joined));
                OnPropertyChanged(nameof(JoinLeaveChannelToolTip));
                ChangedConnectionState?.Invoke(this, EventArgs.Empty);
            }
        }

        private void TwitchChannelManager_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            if (e.Channel.Equals(Name, StringComparison.InvariantCultureIgnoreCase))
            {
                OnPropertyChanged(nameof(CanEditName));
                OnPropertyChanged(nameof(Joined));
                OnPropertyChanged(nameof(JoinLeaveChannelToolTip));
                TwitchConfigManager.Instance.AddChannel(Name);
                ChangedConnectionState?.Invoke(this, EventArgs.Empty);
            }
        }

        private void JoinLeaveChannel()
        {
            if (Joined)
            {
                TwitchChannelManager.Instance.LeaveChannel(Name);
            }
            else
            {
                TwitchChannelManager.Instance.JoinChannel(Name);
            }
        }
    }
}