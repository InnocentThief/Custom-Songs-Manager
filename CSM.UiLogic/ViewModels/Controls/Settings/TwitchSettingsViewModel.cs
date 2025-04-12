using CSM.DataAccess.UserConfiguration;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Controls.Settings
{
    internal class TwitchSettingsViewModel(IServiceLocator serviceLocator, UserConfig userConfig) : BaseViewModel(serviceLocator)
    {
        public bool Available
        {
            get => userConfig.TwitchConfig.Available;
            set
            {
                if (userConfig.TwitchConfig.Available == value)
                    return;
                userConfig.TwitchConfig.Available = value;
                OnPropertyChanged();
            }
        }
    }
}
