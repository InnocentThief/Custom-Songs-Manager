using CSM.Framework.Configuration.UserConfiguration;
using CSM.UiLogic.Properties;
using CSM.UiLogic.Wizards;
using CSM.UiLogic.Workspaces;
using CSM.UiLogic.Workspaces.Infos;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;

namespace CSM.UiLogic
{
    /// <summary>
    /// Represents the main window view model.
    /// </summary>
    public class MainWindowViewModel : ObservableObject
    {
        #region Private fields

        private ObservableCollection<BaseWorkspaceViewModel> workspaces;
        private BaseWorkspaceViewModel selectedWorkspace;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a list of available workspaces.
        /// </summary>
        public ObservableCollection<BaseWorkspaceViewModel> Workspaces
        {
            get
            {
                if (workspaces == null)
                {
                    workspaces = new ObservableCollection<BaseWorkspaceViewModel>()
                    {
                        new CustomLevelsViewModel() { Title = Resources.Workspace_CustomLevels, IconGlyph = "&#xe023;" },
                        new PlaylistsViewModel() { Title = Resources.Workspace_Playlists, IconGlyph = "&#xe029;" },
                        new TwitchIntegrationViewModel() { Title = Resources.Workspace_Twitch, IconGlyph = "&#xe800;" },
                        new ToolsViewModel() { Title = Resources.Workspace_Tools, IconGlyph = "&#xe13c;" }
                    };
                    SelectedWorkspace = workspaces.Single(w => w.WorkspaceType == UserConfigManager.Instance.Config.DefaultWorkspace);
                }
                return workspaces;
            }
        }

        /// <summary>
        /// Gets or sets the selected workspace.
        /// </summary>
        public BaseWorkspaceViewModel SelectedWorkspace
        {
            get
            {
                if (selectedWorkspace == null)
                {
                    selectedWorkspace = workspaces.FirstOrDefault();
                }
                return selectedWorkspace;
            }
            set
            {
                if (value == selectedWorkspace) return;
                selectedWorkspace = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Command used to show the settings panel.
        /// </summary>
        public RelayCommand SettingsCommand { get; }

        /// <summary>
        /// Command used to show the info window.
        /// </summary>
        public RelayCommand InfoCommand { get; }

        #endregion

        /// <summary>
        /// Initializes a new <see cref="MainWindowViewModel"/>.
        /// </summary>
        public MainWindowViewModel()
        {
            //Settings = new SettingsViewModel();
            SettingsCommand = new RelayCommand(ShowSettings);
            InfoCommand = new RelayCommand(ShowInfo);
        }

        /// <summary>
        /// Loads the data of the selected workspace.
        /// </summary>
        public void LoadWorkspace()
        {
            if (selectedWorkspace != null)
            {
                selectedWorkspace.LoadData();
            }
        }

        #region Helper methods

        /// <summary>
        /// Shows the settings panel.
        /// </summary>
        private void ShowSettings()
        {
            var viewModel = new SettingsViewModel();
            EditWindowController.Instance().ShowEditWindow(viewModel);
        }

        /// <summary>
        /// Shows the info window.
        /// </summary>
        private void ShowInfo()
        {
            var viewModel = new EditWindowInfoViewModel();
            EditWindowController.Instance().ShowEditWindow(viewModel);
        }

        #endregion
    }
}