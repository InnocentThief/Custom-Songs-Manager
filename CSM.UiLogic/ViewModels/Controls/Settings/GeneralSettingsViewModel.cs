using CSM.DataAccess.UserConfiguration;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Controls.Settings
{
    internal class GeneralSettingsViewModel(IServiceLocator serviceLocator, UserConfig userConfig) : BaseViewModel(serviceLocator)
    {
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
    }
}
