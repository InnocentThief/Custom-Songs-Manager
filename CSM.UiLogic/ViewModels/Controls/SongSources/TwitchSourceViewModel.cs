using CSM.Business.Core.Twitch;
using CSM.Business.Interfaces;
using CSM.DataAccess.BeatSaver;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;
using CSM.UiLogic.ViewModels.Controls.SongSources.Twitch;
using System.Collections.ObjectModel;
using TwitchLib.Client.Events;

namespace CSM.UiLogic.ViewModels.Controls.SongSources
{
    internal class TwitchSourceViewModel : BaseViewModel, ISongSourceViewModel
    {
        #region Private fields

        private IRelayCommand? loginCommand, logoutCommand, addChannelCommand, removeChannelCommand, connectToTwitchCommand;
        private bool loggedIn;
        private TwitchChannelViewModel? selectedChannel;
        private TwitchSongViewModel? selectedMap;
        private bool connected;

        private readonly IBeatSaverService beatSaverService;
        private readonly ITwitchService twitchService;
        private readonly ITwitchChannelService twitchChannelService;
        private readonly IUserConfigDomain userConfigDomain;

        #endregion

        #region Properties

        public IRelayCommand LoginCommand => loginCommand ??= CommandFactory.CreateFromAsync(LoginToTwitchAsync, CanLoginToTwitch);
        public IRelayCommand LogoutCommand => logoutCommand ??= CommandFactory.Create(LogoutFromTwitch, CanLogoutFromTwitch);
        public IRelayCommand AddChannelCommand => addChannelCommand ??= CommandFactory.Create(AddChannel, CanAddChannel);
        public IRelayCommand RemoveChannelCommand => removeChannelCommand ??= CommandFactory.Create(RemoveChannel, CanRemoveChannel);
        public IRelayCommand ConnectToTwitchCommand => connectToTwitchCommand ??= CommandFactory.Create(ConnectToTwitch, CanConnectToTwitch);

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
                connectToTwitchCommand?.RaiseCanExecuteChanged();
            }
        }

        public bool Connected
        {
            get => connected;
            set
            {
                if (value == connected)
                    return;
                connected = value;
                OnPropertyChanged();
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

        public ObservableCollection<TwitchSongViewModel> Maps { get; } = [];

        public TwitchSongViewModel? SelectedMap
        {
            get => selectedMap;
            set
            {
                if (value == selectedMap)
                    return;
                selectedMap = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public TwitchSourceViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
            beatSaverService = serviceLocator.GetService<IBeatSaverService>();
            twitchService = serviceLocator.GetService<ITwitchService>();
            twitchChannelService = serviceLocator.GetService<ITwitchChannelService>();
            twitchChannelService.OnBsrKeyReceived += TwitchChannelService_OnBsrKeyReceived;
            twitchChannelService.OnConnected += TwitchChannelService_OnConnected;
            userConfigDomain = serviceLocator.GetService<IUserConfigDomain>();

            foreach (var channel in userConfigDomain.Config?.TwitchConfig.Channels ?? [])
            {
                var newChannel = new TwitchChannelViewModel(serviceLocator)
                {
                    Name = channel.Name,
                };
                newChannel.OnRemoveChannel += Channel_OnRemove;
                Channels.Add(newChannel);
            }
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

            var songKeys = userConfigDomain.Config?.TwitchConfig.Songs.Select(x => x.Key).ToList();
            if (songKeys == null || songKeys.Count == 0)
                return;
            var mapDetails = await beatSaverService.GetMapDetailsAsync(songKeys, BeatSaverKeyType.Id);
            foreach (var song in userConfigDomain.Config?.TwitchConfig.Songs ?? [])
            {
                if (mapDetails == null || !mapDetails.ContainsKey(song.Key))
                    continue;
                var mapDetail = mapDetails[song.Key];
                var newSong = new TwitchSongViewModel(ServiceLocator, song.ChannelName, mapDetail);
                newSong.OnRemoveSong += Song_OnRemoveSong;
                Maps.Add(newSong);
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
            var newChannel = new TwitchChannelViewModel(ServiceLocator);
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

        private async void TwitchChannelService_OnBsrKeyReceived(object? sender, SongRequestEventArgs e)
        {
            var mapDetail = await beatSaverService.GetMapDetailAsync(e.Key, BeatSaverKeyType.Id);
            if (mapDetail == null)
                return;

            var song = new TwitchSongViewModel(ServiceLocator, e.ChannelName, mapDetail);
            song.OnRemoveSong += Song_OnRemoveSong;
            twitchChannelService.AddSong(e.Key, e.ChannelName, song.ReceivedAt);
            Maps.Add(song);
        }

        private void Song_OnRemoveSong(object? sender, EventArgs e)
        {
            var song = sender as TwitchSongViewModel;
            if (song == null)
                return;
            song.OnRemoveSong -= Song_OnRemoveSong;
            song.CleanUpReferences();
            twitchChannelService.RemoveSong(song.BsrKey);
            Maps.Remove(song);
        }

        private void Channel_OnRemove(object? sender, EventArgs e)
        {
            var channel = sender as TwitchChannelViewModel;
            if (channel == null)
                return;

            channel.OnRemoveChannel -= Channel_OnRemove;
            channel.CleanupReferences();
            Channels.Remove(channel);
        }

        private void ConnectToTwitch()
        {
            SetLoadingInProgress(true, "Connecting to Twitch...");
            twitchChannelService.Initialize();
        }

        private bool CanConnectToTwitch()
        {
            return LoggedIn;
        }

        private void TwitchChannelService_OnConnected(object? sender, OnConnectedArgs e)
        {
            SetLoadingInProgress(false, string.Empty);
            Connected = true;
        }

        #endregion
    }
}
