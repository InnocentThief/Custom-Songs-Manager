using CSM.Business.TwitchIntegration;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.TwitchIntegration
{
    /// <summary>
    /// Represents one configured twitch channel.
    /// </summary>
    public class TwitchChannelViewModel : ObservableObject
    {
        private string channelName;

        public Guid Key { get; }

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

        public bool CanEditName
        {
            get
            {
                // TODO: return !TwitchChannelConnectionManager.ChannelConnected
                return true;
            }
        }

        public RelayCommand ConnectCommand { get; }



        public string CommandText
        {
            get
            {
                // TODO: Check connection state on TwitchChannelConnectionManager
                if (true) return "Disconnect";
                return "Connect";
            }
        }

        public TwitchChannelViewModel(Guid key)
        {
            ConnectCommand = new RelayCommand(ChangeConnectionState);
            //TwitchChannelManager.Instance.AddChannel(Key, "InnocentThief");

        }

        private void ChangeConnectionState()
        {

        }
    }
}