using CSM.Business.Interfaces;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;
using CSM.UiLogic.ViewModels.Controls.SongSources.Twitch;
using System.Collections.ObjectModel;

namespace CSM.UiLogic.ViewModels.Controls.SongSources
{
    internal class TwitchSourceViewModel : BaseViewModel, ISongSourceViewModel
    {
        #region Private fields

        private IRelayCommand? loginCommand, logoutCommand, addChannelCommand, removeChannelCommand;
        private bool loggedIn;
        private TwitchChannelViewModel? selectedChannel;

        private readonly ITwitchService twitchService;
        private readonly IUserConfigDomain userConfigDomain;

        #endregion

        #region Properties

        public IRelayCommand LoginCommand => loginCommand ??= CommandFactory.CreateFromAsync(LoginToTwitchAsync, CanLoginToTwitch);
        public IRelayCommand LogoutCommand => logoutCommand ??= CommandFactory.Create(LogoutFromTwitch, CanLogoutFromTwitch);
        public IRelayCommand AddChannelCommand => addChannelCommand ??= CommandFactory.Create(AddChannel, CanAddChannel);
        public IRelayCommand RemoveChannelCommand => removeChannelCommand ??= CommandFactory.Create(RemoveChannel, CanRemoveChannel);

        public bool LoggedIn
        {
            get => loggedIn;
            set
            {
                if (value == loggedIn)
                    return;
                loggedIn = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Username));

                loginCommand?.RaiseCanExecuteChanged();
                logoutCommand?.RaiseCanExecuteChanged();
                addChannelCommand?.RaiseCanExecuteChanged();
                removeChannelCommand?.RaiseCanExecuteChanged();
            }
        }

        public string Username
        {
            get
            {
                if (loggedIn)
                {
                    return $"Logged in to Twitch as {userConfigDomain.Config?.TwitchConfig.Username ?? string.Empty}";
                }
                return "Not logged in to Twitch";
            }
        }

        public ObservableCollection<TwitchChannelViewModel> Channels { get; } = [];

        public TwitchChannelViewModel? SelectedChannel
        {
            get => selectedChannel;
            set
            {
                if (value == selectedChannel)
                    return;
                selectedChannel = value;
                OnPropertyChanged();
                RemoveChannelCommand?.RaiseCanExecuteChanged();
            }
        }

        #endregion

        public TwitchSourceViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
            twitchService = serviceLocator.GetService<ITwitchService>();
            userConfigDomain = serviceLocator.GetService<IUserConfigDomain>();
        }

        public async Task LoadAsync()
        {
            var valid = await twitchService.ValidateAsync();
            if (valid)
            {
                LoggedIn = true;
            }
            else
            {
                LoggedIn = false;
            }
        }

        #region Helper methods

        private async Task LoginToTwitchAsync()
        {
            var valid = await twitchService.ValidateAsync();
            if (!valid)
            {
                await twitchService.GetAccessTokenAsync();
                await Task.Delay(2000);
                valid = await twitchService.ValidateAsync();
                if (!valid)
                {
                    UserInteraction.ShowError("Unable to login in to Twitch");
                    LoggedIn = false;
                }
            }

            LoggedIn = true;
        }

        private bool CanLoginToTwitch()
        {
            return !LoggedIn;
        }

        private void LogoutFromTwitch()
        {
            userConfigDomain.Config!.TwitchConfig.AccessToken = string.Empty;
            userConfigDomain.Config!.TwitchConfig.RefreshToken = string.Empty;
            userConfigDomain.Config!.TwitchConfig.UserId = string.Empty;
            userConfigDomain.Config!.TwitchConfig.Login = string.Empty;
            userConfigDomain.Config!.TwitchConfig.Username = string.Empty;
            userConfigDomain.SaveUserConfig();

            LoggedIn = false;
        }

        private bool CanLogoutFromTwitch()
        {
            return LoggedIn;
        }

        private void AddChannel()
        {
            var newChannel = new TwitchChannelViewModel();
            Channels.Add(newChannel);

        }

        private bool CanAddChannel()
        {
            return LoggedIn;
        }

        private void RemoveChannel()
        {
        }

        private bool CanRemoveChannel()
        {
            return LoggedIn && SelectedChannel != null;
        }

        #endregion
    }
}
