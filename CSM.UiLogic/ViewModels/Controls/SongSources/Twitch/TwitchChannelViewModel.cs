using CSM.Business.Interfaces;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;
using System.Windows.Threading;
using TwitchLib.Client.Events;

namespace CSM.UiLogic.ViewModels.Controls.SongSources.Twitch
{
    internal class TwitchChannelViewModel : BaseViewModel
    {
        #region Private fields

        private IRelayCommand? joinCommand, leaveCommand, removeCommand;
        private bool joined = false;
        private string name = string.Empty;

        private readonly ITwitchChannelService twitchChannelService;

        #endregion

        #region Properties

        public IRelayCommand? JoinCommand => joinCommand ??= CommandFactory.Create(Join, CanJoin);

        public IRelayCommand? LeaveCommand => leaveCommand ??= CommandFactory.Create(Leave, CanLeave);

        public IRelayCommand? RemoveCommand => removeCommand ??= CommandFactory.Create(Remove, CanRemove);

        public bool Joined => twitchChannelService.CheckChannelIsJoined(Name);

        public string Name
        {
            get => name;
            set
            {
                if (value == name)
                    return;
                name = value;
                OnPropertyChanged();
                JoinCommand?.RaiseCanExecuteChanged();
            }
        }

        #endregion

        public TwitchChannelViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
            twitchChannelService = serviceLocator.GetService<ITwitchChannelService>();
            twitchChannelService.OnJoinedChannel += TwitchChannelService_OnJoinedChannel;
            twitchChannelService.OnLeftChannel += TwitchChannelService_OnLeftChannel;
        }

        public void CleanupReferences()
        {
            twitchChannelService.OnJoinedChannel -= TwitchChannelService_OnJoinedChannel;
            twitchChannelService.OnLeftChannel -= TwitchChannelService_OnLeftChannel;
        }

        #region Helper methods

        private void Join()
        {
            if (!twitchChannelService.Initialize())
                return;
            twitchChannelService.JoinChannel(Name);
        }

        private bool CanJoin()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }

        private void Leave()
        {
            if (!twitchChannelService.Initialize())
                return;
            twitchChannelService.LeaveChannel(Name);
        }

        private bool CanLeave()
        {
            return true;
        }

        private void Remove()
        {

        }

        private bool CanRemove()
        {
            return true;
        }

        private void TwitchChannelService_OnJoinedChannel(object? sender, OnJoinedChannelArgs e)
        {
            if (e.Channel.Equals(Name, StringComparison.InvariantCultureIgnoreCase))
            {
                OnPropertyChanged(nameof(Joined));
            }
        }

        private void TwitchChannelService_OnLeftChannel(object? sender, OnLeftChannelArgs e)
        {
            if (e.Channel.Equals(Name, StringComparison.InvariantCultureIgnoreCase))
            {
                OnPropertyChanged(nameof(Joined));
            }
        }

        #endregion
    }
}
