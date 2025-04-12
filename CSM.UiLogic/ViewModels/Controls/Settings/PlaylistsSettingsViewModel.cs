using CSM.DataAccess.UserConfiguration;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Controls.Settings
{
    internal class PlaylistsSettingsViewModel(IServiceLocator serviceLocator, UserConfig userConfig) : BaseViewModel(serviceLocator)
    {
        public bool Available
        {
            get => userConfig.PlaylistsConfig.Available;
            set
            {
                if (value == userConfig.PlaylistsConfig.Available)
                    return;
                userConfig.PlaylistsConfig.Available = value;
                OnPropertyChanged();
            }
        }

        public string PlaylistsPath
        {
            get => userConfig.PlaylistsConfig.PlaylistPath.Path;
            set
            {
                if (value == userConfig.PlaylistsConfig.PlaylistPath.Path)
                    return;
                userConfig.PlaylistsConfig.PlaylistPath.Path = value;
                OnPropertyChanged();
            }
        }
    }
}
