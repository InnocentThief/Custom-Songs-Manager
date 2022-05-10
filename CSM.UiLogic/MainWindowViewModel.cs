using CSM.UiLogic.Workspaces;
using Microsoft.Toolkit.Mvvm.ComponentModel;
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
        private ObservableCollection<BaseWorkspaceViewModel> workspaces;
        private BaseWorkspaceViewModel selectedWorkspace;

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

        public void LoadWorkspace()
        {
            if (selectedWorkspace != null)
            {
                selectedWorkspace.LoadData();
            }
        }
    }
}