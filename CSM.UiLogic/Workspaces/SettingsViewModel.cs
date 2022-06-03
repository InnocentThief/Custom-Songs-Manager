using CSM.UiLogic.Properties;
using CSM.UiLogic.Wizards;
using CSM.UiLogic.Workspaces.Settings;

namespace CSM.UiLogic.Workspaces
{
    /// <summary>
    /// ViewModel for the settings workspace.
    /// </summary>
    public class SettingsViewModel : EditWindowBaseViewModel
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
        /// Gets the height of the window.
        /// </summary>
        public override int Height => 400;

        /// <summary>
        /// Gets the width of the window.
        /// </summary>
        public override int Width => 800;

        /// <summary>
        /// Gets the title of the window.
        /// </summary>
        public override string Title => Resources.Settings_Title;

        #endregion

        /// <summary>
        /// Initializes a new <see cref="SettingsViewModel"/>.
        /// </summary>
        public SettingsViewModel(): base(string.Empty, string.Empty)
        {
            BeatSaberSettings = new BeatSaberSettingsViewModel();
            WorkspaceSettings = new WorkspaceSettingsViewModel();
        }
    }
}