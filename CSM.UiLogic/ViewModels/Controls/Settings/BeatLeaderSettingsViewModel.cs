using CSM.DataAccess.UserConfiguration;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Controls.Settings
{
    internal class BeatLeaderSettingsViewModel(IServiceLocator serviceLocator, UserConfig userConfig) : BaseViewModel(serviceLocator)
    {
        public bool Available
        {
            get => userConfig.BeatLeaderConfig.Available;
            set
            {
                if (userConfig.BeatLeaderConfig.Available == value)
                    return;
                userConfig.BeatLeaderConfig.Available = value;
                OnPropertyChanged();
            }
        }
    }
}
