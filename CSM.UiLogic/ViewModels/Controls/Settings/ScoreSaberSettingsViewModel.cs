using CSM.DataAccess.UserConfiguration;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Controls.Settings
{
    internal class ScoreSaberSettingsViewModel(IServiceLocator serviceLocator, UserConfig userConfig) : BaseViewModel(serviceLocator)
    {
        public bool Available
        {
            get => userConfig.ScoreSaberConfig.Available;
            set
            {
                if (userConfig.ScoreSaberConfig.Available == value)
                    return;
                userConfig.ScoreSaberConfig.Available = value;
                OnPropertyChanged();
            }
        }
    }
}
