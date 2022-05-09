using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces
{
    public class CustomLevelsViewModel : BaseWorkspaceViewModel
    {
        public override WorkspaceType WorkspaceType => WorkspaceType.CustomLevels;

        public override async Task LoadDataAsync()
        {
          await Task.CompletedTask;
        }

        public override void UnloadData()
        {
       
        }
    }
}
