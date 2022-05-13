using CSM.Framework.Configuration;
using CSM.UiLogic.Workspaces;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic
{
    public class MainWindowViewModel : ObservableObject
    {
        #region Private fields

        private ObservableCollection<BaseWorkspaceViewModel> workspaces;
        private BaseWorkspaceViewModel selectedWorkspace;


        #endregion

        #region Public Properties

        public ObservableCollection<BaseWorkspaceViewModel> Workspaces
        {
            get
            {
                if (workspaces == null)
                {
                    workspaces = new ObservableCollection<BaseWorkspaceViewModel>()
                    {
                        new CustomLevelsViewModel() { Title = "Custom Levels", IconGlyph = "&#xe023;" },
                        new PlaylistsViewModel() { Title = "Playlists", IconGlyph = "&#xe029;" },
                        new TwitchIntegrationViewModel() { Title = "Twitch", IconGlyph = "&#xe800;" },
                        new ToolsViewModel() { Title = "Tools", IconGlyph = "&#xe13c;" }
                    };
                    SelectedWorkspace = workspaces.Single(w=>w.WorkspaceType == UserConfigManager.Instance.Config.DefaultWorkspace);
                }
                return workspaces;
            }
        }

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

        public RelayCommand SettingsCommand { get; }

        public SettingsViewModel Settings { get; }

        #endregion

        public MainWindowViewModel()
        {
            Settings = new SettingsViewModel();
            SettingsCommand = new RelayCommand(ShowSettings);
        }

        public void LoadWorkspace()
        {
            if (selectedWorkspace != null)
            {
                selectedWorkspace.LoadData();
            }
        }

        public void ShowSettings()
        {
            Settings.Visible = true;
        }
    }
}