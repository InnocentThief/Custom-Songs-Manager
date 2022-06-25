using CSM.Framework.Configuration.UserConfiguration;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace CSM.UiLogic.Workspaces.Settings
{
    /// <summary>
    /// Contains the settings related to BeatSaver.com
    /// </summary>
    public class BeatSaverSettingsViewModel : ObservableObject
    {
        private string beatSaverAPIEndpoint;

        /// <summary>
        /// Gets or sets the API endpoint for BeatSaver.com
        /// </summary>
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

        /// <summary>
        /// Initializes a new <see cref="BeatSaverAPIEndpoint"/>.
        /// </summary>
        public BeatSaverSettingsViewModel()
        {
            beatSaverAPIEndpoint = UserConfigManager.Instance.Config.BeatSaverAPIEndpoint;
        }
    }
}