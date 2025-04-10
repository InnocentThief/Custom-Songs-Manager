using CSM.Business.Interfaces;
using CSM.Framework;
using CSM.Framework.ServiceLocation;
using CSM.Framework.Types;
using CSM.UiLogic.AbstractBase;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CSM.UiLogic.ViewModels.Navigation
{
    internal class NavigationViewModel : BaseViewModel
    {
        #region Public Properties

        public ObservableCollection<NavigationItemViewModel> Items { get; } = [];

        public ICommand ShowInfoCommand { get; }

        public ICommand ShowSettingsCommand { get; }

        #endregion

        public event EventHandler? SelectionChanged;

        public NavigationViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
            var customLevels = new NavigationItemViewModel(serviceLocator, NavigationType.CustomLevels, "Custom Levels", "&#xe023;");
            customLevels.SelectionChanged += NavigationItemSelectionChanged;
            Items.Add(customLevels);
            var playlists = new NavigationItemViewModel(serviceLocator, NavigationType.Playlists, "Playlists", "&#xe029;");
            playlists.SelectionChanged += NavigationItemSelectionChanged;
            Items.Add(playlists);
            var twitch = new NavigationItemViewModel(serviceLocator, NavigationType.TwitchIntegration, "Twitch", "&#xe800;");
            twitch.SelectionChanged += NavigationItemSelectionChanged;
            Items.Add(twitch);
            var scoreSaber = new NavigationItemViewModel(serviceLocator, NavigationType.ScoreSaberIntegration, "ScoreSaber", "&#xea0b;");
            scoreSaber.SelectionChanged += NavigationItemSelectionChanged;
            Items.Add(scoreSaber);
            var beatLeader = new NavigationItemViewModel(serviceLocator, NavigationType.BeatLeaderIntegration, "BeatLeader", "&#xea00;");
            beatLeader.SelectionChanged += NavigationItemSelectionChanged;
            Items.Add(beatLeader);

            //ShowInfoCommand = CommandFactory. // Do something nice here
        }

        public void NavigateToDefaultWorkspace()
        {
            var defaultNavigationItem = ServiceLocator.GetService<IUserConfigDomain>().Config?.DefaultWorkspace ?? NavigationType.CustomLevels;
            var selectedNavigationItem = Items.Single(n => n.NavigationType == defaultNavigationItem);
            selectedNavigationItem.IsSelected = true;

            //SelectionChanged?.Invoke(selectedNavigationItem, EventArgs.Empty);
        }

        private void NavigationItemSelectionChanged(object? sender, EventArgs e)
        {
            SelectionChanged?.Invoke(sender, e);
        }

        private void ShowInfo()
        {

        }

        private void ShowSettings()
        {
        }
    }
}
