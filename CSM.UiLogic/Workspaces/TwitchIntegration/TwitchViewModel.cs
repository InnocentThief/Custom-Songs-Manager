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

            }
        }

        public TwitchViewModel()
        {
            Channels = new ObservableCollection<TwitchChannelViewModel>();

            AddChannelCommand = new RelayCommand(AddChannel);
            RemoveChannelCommand = new RelayCommand(RemoveChannel);

            Channels.Add(new TwitchChannelViewModel() { ChannelName = "InnocentThief" });
            Channels.Add(new TwitchChannelViewModel() { ChannelName = "GoodOldNervy" });
        }

        private void AddChannel()
        {

        }

        private void RemoveChannel()
        {

        }
    }
}