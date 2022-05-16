using CSM.UiLogic.Workspaces.Settings;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace CSM.UiLogic.Workspaces
{
    /// <summary>
    /// ViewModel for the settings workspace.
    /// </summary>
    public class SettingsViewModel : ObservableObject
    {
        private bool visible;

        #region Public Properties

        /// <summary>
        /// Gets the Beat Saber settings.
        /// </summary>
        public BeatSaberSettingsViewModel BeatSaberSettings { get; }

        /// <summary>
        /// Gets the workspace settings.
        /// </summary>
        public WorkspaceSettingsViewModel WorkspaceSettings { get; }

        /// <summary>
        /// Gets or sets whether the settings panel is visible.
        /// </summary>
        public bool Visible
        {
            get => visible;
            set
            {
                if (value == visible) return;
                visible = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Command used to close the settings workspace.
        /// </summary>
        public RelayCommand CloseCommand { get; }

        #endregion

        /// <summary>
        /// Initializes a new <see cref="SettingsViewModel"/>.
        /// </summary>
        public SettingsViewModel()
        {
            CloseCommand = new RelayCommand(Close);
            BeatSaberSettings = new BeatSaberSettingsViewModel();
            WorkspaceSettings = new WorkspaceSettingsViewModel();
        }

        private void Close()
        {
            Visible = false;
        }
    }
}