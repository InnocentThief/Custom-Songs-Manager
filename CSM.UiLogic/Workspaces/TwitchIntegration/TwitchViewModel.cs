using CSM.Business.TwitchIntegration;
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
        //private TwitchConnectionManager twitchConnectionManager;
        private TwitchChannelViewModel selectedChannel;

        #region Public Properties

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

        #endregion

        public TwitchViewModel()
        {
            Channels = new ObservableCollection<TwitchChannelViewModel>();

            AddChannelCommand = new RelayCommand(AddChannel);
            RemoveChannelCommand = new RelayCommand(RemoveChannel, CanRemoveChannel);

            //var twitchConnectionManager = TwitchConnectionManager.Instance;
        }

        #region Helper methods

        private void AddChannel()
        {
            var newChannel = new TwitchChannelViewModel(Guid.NewGuid());
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