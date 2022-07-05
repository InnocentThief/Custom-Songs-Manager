using CSM.Business.TwitchIntegration;
using CSM.Business.TwitchIntegration.TwitchConfiguration;
using CSM.Framework.Logging;
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

        /// <summary>
        /// Gets or sets the name of the channel.
        /// </summary>
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

        /// <summary>
        /// Gets whether the channel name can be edited.
        /// </summary>
        public bool CanEditName => !TwitchChannelManager.Instance.CheckChannelIsJoined(Name);

        /// <summary>
        /// Command used to join or leave the channel.
        /// </summary>
        public RelayCommand JoinLeaveChannelCommand { get; }

        /// <summary>
        /// ToolTip text for <see cref="JoinLeaveChannelCommand"/>.
        /// </summary>
        public string JoinLeaveChannelToolTip
        {
            get
            {
                if (Joined) return "Leave channel";
                return "Join channel";
            }
        }

        public RelayCommand ScoreSaberCommand { get; }

        /// <summary>
        /// Gets whether the channel is joined.
        /// </summary>
        public bool Joined => TwitchChannelManager.Instance.CheckChannelIsJoined(Name);

        #endregion

        /// <summary>
        /// Occurs on connection change.
        /// </summary>
        public event EventHandler ChangedConnectionState;

        public event EventHandler<ScoreSaberAddPlayerEventArgs> OnScoreSaberAddPlayer;

        /// <summary>
        /// Initializes a new <see cref="TwitchChannelViewModel"/>.
        /// </summary>
        public TwitchChannelViewModel()
        {
            JoinLeaveChannelCommand = new RelayCommand(JoinLeaveChannel);
            ScoreSaberCommand = new RelayCommand(ShowInScoreSaber);
            TwitchChannelManager.OnJoinedChannel += this.TwitchChannelManager_OnJoinedChannel;
            TwitchChannelManager.OnLeftChannel += TwitchChannelManager_OnLeftChannel;
        }

        #region Helper methods

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
                LoggerProvider.Logger.Info<TwitchChannelViewModel>($"Leaving Twitch channel {Name}");
                TwitchChannelManager.Instance.LeaveChannel(Name);
            }
            else
            {
                LoggerProvider.Logger.Info<TwitchChannelViewModel>($"Joining Twitch channel {Name}");
                TwitchChannelManager.Instance.JoinChannel(Name);
            }
        }

        private void ShowInScoreSaber()
        {
            OnScoreSaberAddPlayer?.Invoke(this, new ScoreSaberAddPlayerEventArgs { Playername = Name });
        }

        #endregion
    }
}