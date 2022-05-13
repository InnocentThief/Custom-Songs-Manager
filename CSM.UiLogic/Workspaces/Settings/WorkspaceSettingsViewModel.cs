using CSM.Framework;
using CSM.Framework.Configuration;
using CSM.Framework.Extensions;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSM.UiLogic.Workspaces.Settings
{
    public class WorkspaceSettingsViewModel : ObservableObject
    {
        private WorkspaceViewModel workspaceViewModel;

        public List<WorkspaceViewModel> Workspaces { get; }

        public WorkspaceViewModel SelectedWorkspace
        {
            get => workspaceViewModel;
            set
            {
                if (workspaceViewModel == value) return;
                workspaceViewModel = value;
                UserConfigManager.Instance.Config.DefaultWorkspace = workspaceViewModel.Type;
                UserConfigManager.Instance.SaveUserConfig();
                OnPropertyChanged();
            }
        }

        public WorkspaceSettingsViewModel()
        {
            Workspaces = new List<WorkspaceViewModel>();
            foreach (var workspaceType in Enum.GetValues(typeof(WorkspaceType)))
            {
                Workspaces.Add(new WorkspaceViewModel(workspaceType.ToString().ToWorkspaceType(), (WorkspaceType)workspaceType));
            }
            SelectedWorkspace = Workspaces.SingleOrDefault(w => w.Type == UserConfigManager.Instance.Config.DefaultWorkspace);
        }
    }
}