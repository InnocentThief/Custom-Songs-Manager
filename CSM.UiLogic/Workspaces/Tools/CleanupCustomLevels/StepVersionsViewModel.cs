using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.Tools.CleanupCustomLevels
{
    public class StepVersionsViewModel : StepBaseViewModel
    {
        public StepVersionsViewModel() : base("Version", "Cleanup old versions")
        {
        }

        public override async Task LoadDataAsync()
        {
            await Task.CompletedTask;
        }
    }
}
