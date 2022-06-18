using CSM.Framework.Configuration.UserConfiguration;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace CSM.UiLogic.Workspaces.Settings
{
    public class BeatSaverSettingsViewModel : ObservableObject
    {
        private string beatSaverAPIEndpoint;

        public string BeatSaverAPIEndpoint
        {
            get => beatSaverAPIEndpoint;
            set
            {
                if (value == beatSaverAPIEndpoint) return;
                beatSaverAPIEndpoint = value;
                UserConfigManager.Instance.Config.BeatSaverAPIEndpoint = beatSaverAPIEndpoint;
                UserConfigManager.Instance.SaveUserConfig();
                OnPropertyChanged();
            }
        }

        public BeatSaverSettingsViewModel()
        {
            beatSaverAPIEndpoint = UserConfigManager.Instance.Config.BeatSaverAPIEndpoint;
        }
    }
}