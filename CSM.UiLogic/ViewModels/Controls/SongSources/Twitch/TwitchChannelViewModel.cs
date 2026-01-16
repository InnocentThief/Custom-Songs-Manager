using CSM.Business.Interfaces;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;
using TwitchLib.Client.Events;

namespace CSM.UiLogic.ViewModels.Controls.SongSources.Twitch
{
    internal class TwitchChannelViewModel : BaseViewModel
    {
        #region Private fields

        private IRelayCommand? joinCommand, leaveCommand, removeCommand;
        private string name = string.Empty;

        private readonly ITwitchChannelService twitchChannelService;

        #endregion

        #region Properties

        public IRelayCommand? JoinCommand => joinCommand ??= CommandFactory.CreateFromAsync(JoinAsync, CanJoin);

        public IRelayCommand? LeaveCommand => leaveCommand ??= CommandFactory.CreateFromAsync(LeaveAsync, CanLeave);

        public IRelayCommand? RemoveCommand => removeCommand ??= CommandFactory.CreateFromAsync(RemoveAsync, CanRemove);

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

        public event EventHandler? OnRemoveChannel;

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

        private async Task JoinAsync()
        {
            if (!await twitchChannelService.Initialize())
                return;
            await twitchChannelService.JoinChannelAsync(Name);
        }

        private bool CanJoin()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }

        private async Task LeaveAsync()
        {
            if (!await twitchChannelService.Initialize())
                return;
            await twitchChannelService.LeaveChannelAsync(Name);
        }

        private bool CanLeave()
        {
            return true;
        }

        private async Task RemoveAsync()
        {
            if (Joined)
                await twitchChannelService.LeaveChannelAsync(Name);
            twitchChannelService.RemoveChannel(Name);
            OnRemoveChannel?.Invoke(this, EventArgs.Empty);
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
                twitchChannelService.AddChannel(Name);
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
