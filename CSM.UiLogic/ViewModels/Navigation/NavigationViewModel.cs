using CSM.Business.Interfaces;
using CSM.Framework.ServiceLocation;
using CSM.Framework.Types;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.ViewModels.Controls.Settings;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CSM.UiLogic.ViewModels.Navigation
{
    internal class NavigationViewModel : BaseViewModel
    {
        private readonly IUserConfigDomain userConfigDomain;

        #region Public Properties

        public ObservableCollection<NavigationItemViewModel> Items { get; } = [];

        public ICommand ShowInfoCommand { get; }

        public ICommand ShowSettingsCommand { get; }

        #endregion

        public event EventHandler? SelectionChanged;

        public NavigationViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
            userConfigDomain = serviceLocator.GetService<IUserConfigDomain>();

            if (userConfigDomain.Config?.CustomLevelsConfig.Available ?? false)
            {
                var customLevels = new NavigationItemViewModel(serviceLocator, NavigationType.CustomLevels, "Custom Levels", "&#xe023;");
                customLevels.SelectionChanged += NavigationItemSelectionChanged;
                Items.Add(customLevels);
            }
            if (userConfigDomain.Config?.PlaylistsConfig.Available ?? false)
            {
                var playlists = new NavigationItemViewModel(serviceLocator, NavigationType.Playlists, "Playlists", "&#xe029;");
                playlists.SelectionChanged += NavigationItemSelectionChanged;
                Items.Add(playlists);
            }
            if (userConfigDomain.Config?.TwitchConfig.Available ?? false)
            {
                var twitch = new NavigationItemViewModel(serviceLocator, NavigationType.TwitchIntegration, "Twitch", "&#xe800;");
                twitch.SelectionChanged += NavigationItemSelectionChanged;
                Items.Add(twitch);
            }
            if (userConfigDomain.Config?.ScoreSaberConfig.Available ?? false)
            {
                var scoreSaber = new NavigationItemViewModel(serviceLocator, NavigationType.ScoreSaberIntegration, "ScoreSaber", "&#xea0b;");
                scoreSaber.SelectionChanged += NavigationItemSelectionChanged;
                Items.Add(scoreSaber);
            }
            if (userConfigDomain.Config?.BeatLeaderConfig.Available ?? false)
            {
                var beatLeader = new NavigationItemViewModel(serviceLocator, NavigationType.BeatLeaderIntegration, "BeatLeader", "&#xea00;");
                beatLeader.SelectionChanged += NavigationItemSelectionChanged;
                Items.Add(beatLeader);
            }

            ShowInfoCommand = CommandFactory.Create(ShowInfo, CanShowInfo);
            ShowSettingsCommand = CommandFactory.Create(ShowSettings, CanShowSettings);
        }

        public void NavigateToDefaultWorkspace()
        {
            var defaultNavigationItem = userConfigDomain.Config?.DefaultWorkspace ?? NavigationType.CustomLevels;
            var selectedNavigationItem = Items.SingleOrDefault(n => n.NavigationType == defaultNavigationItem);
            if (selectedNavigationItem == null)
                return;
            selectedNavigationItem.IsSelected = true;

            //SelectionChanged?.Invoke(selectedNavigationItem, EventArgs.Empty);
        }

        private void NavigationItemSelectionChanged(object? sender, EventArgs e)
        {
            SelectionChanged?.Invoke(sender, e);
        }

        private void ShowInfo()
        {
            var infoVieWModel = new InfoViewModel(ServiceLocator);
            UserInteraction.ShowWindow<InfoViewModel>(infoVieWModel);
        }

        private bool CanShowInfo()
        {
            return true;
        }

        private void ShowSettings()
        {
            var settingsControlViewModel = new SettingsControlViewModel(ServiceLocator);
            UserInteraction.ShowWindow<SettingsControlViewModel>(settingsControlViewModel);
            var userConfigDomain = ServiceLocator.GetService<IUserConfigDomain>();
            if (settingsControlViewModel.Continue)
            {
                userConfigDomain.SaveUserConfig();
            }
            else
            {
                userConfigDomain.LoadOrCreateUserConfig();
            }
        }

        private bool CanShowSettings()
        {
            return true;
        }
    }
}
