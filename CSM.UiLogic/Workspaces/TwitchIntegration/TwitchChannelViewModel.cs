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
    public class TwitchChannelViewModel
    {

        public Guid Key { get; }

        public string ChannelName { get; set; }

        public bool CanEditName
        {
            get
            {
                // TODO: return !TwitchChannelConnectionManager.ChannelConnected
                return true;
            }
        }

        public RelayCommand ConnectionCommand { get; }

        public string CommandText
        {
            get
            {
                // TODO: Check connection state on TwitchChannelConnectionManager
                if (true) return "Disconnect";
                return "Connect";
            }
        }

        public TwitchChannelViewModel()
        {
            ConnectionCommand = new RelayCommand(ChangeConnection);
        }

        private void ChangeConnection()
        {

        }
    }
}