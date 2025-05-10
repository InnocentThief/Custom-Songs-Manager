using System.Collections.ObjectModel;
using CSM.DataAccess.UserConfiguration;
using CSM.Framework.Extensions;
using CSM.Framework.ServiceLocation;
using CSM.Framework.Types;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Helper;

namespace CSM.UiLogic.ViewModels.Controls.Settings
{
    internal class GeneralSettingsViewModel : BaseViewModel
    {
        private readonly UserConfig userConfig;

        public string BeatSaberInstallationPath
        {
            get => userConfig.BeatSaberInstallPath;
            set
            {
                if (value == userConfig.BeatSaberInstallPath)
                    return;
                userConfig.BeatSaberInstallPath = value;
                OnPropertyChanged();
            }
        }

        public string BeatSaverAPIEndpoint
        {
            get => userConfig.BeatSaverAPIEndpoint;
            set
            {
                if (value == userConfig.BeatSaverAPIEndpoint)
                    return;
                userConfig.BeatSaverAPIEndpoint = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<EnumWrapper<NavigationType>> Workspaces { get; } = [];

        public EnumWrapper<NavigationType>? SelectedWorkspace
        {
            get => Workspaces.SingleOrDefault(w => w.Value == userConfig.DefaultWorkspace);
            set
            {
                if (value == null || value.Value == userConfig.DefaultWorkspace)
                    return;
                userConfig.DefaultWorkspace = value.Value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<EnumWrapper<LeaderboardType>> VisibleLeaderboardInfos { get; } = [];

        public EnumWrapper<LeaderboardType>? SelectedVisibleLeaderboardInfos
        {
            get => VisibleLeaderboardInfos.SingleOrDefault(w => w.Value == userConfig.VisibleLeaderboardInfos);
            set
            {
                if (value == null || value.Value == userConfig.VisibleLeaderboardInfos)
                    return;
                userConfig.VisibleLeaderboardInfos = value.Value;
                OnPropertyChanged();
            }
        }

        public GeneralSettingsViewModel(IServiceLocator serviceLocator, UserConfig userConfig) : base(serviceLocator)
        {
            this.userConfig = userConfig;

            Workspaces.AddRange(EnumWrapper<NavigationType>.GetValues(serviceLocator, n => n.Value));
            VisibleLeaderboardInfos.AddRange(EnumWrapper<LeaderboardType>.GetValues(serviceLocator));
        }
    }
}
