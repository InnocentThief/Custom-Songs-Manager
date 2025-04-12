using CSM.DataAccess.UserConfiguration;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Controls.Settings
{
    internal class CustomLevelsSettingsViewModel(IServiceLocator serviceLocator, UserConfig userConfig) : BaseViewModel(serviceLocator)
    {
        public bool Available
        {
            get => userConfig.CustomLevelsConfig.Available;
            set
            {
                if (value == userConfig.CustomLevelsConfig.Available)
                    return;
                userConfig.CustomLevelsConfig.Available = value;
                OnPropertyChanged();
            }
        }

        public string CustomLevelsPath
        {
            get => userConfig.CustomLevelsConfig.CustomLevelPath.Path;
            set
            {
                if (value == userConfig.CustomLevelsConfig.CustomLevelPath.Path)
                    return;
                userConfig.CustomLevelsConfig.CustomLevelPath.Path = value;
                OnPropertyChanged();
            }
        }
    }
}
