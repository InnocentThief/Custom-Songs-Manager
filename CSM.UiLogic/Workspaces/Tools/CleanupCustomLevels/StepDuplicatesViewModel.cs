using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.Tools.CleanupCustomLevels
{
    public class StepDuplicatesViewModel : StepBaseViewModel
    {
        public StepDuplicatesViewModel() : base("Duplicates", "Cleanup duplicate custom levels")
        {
        }

        public override async Task LoadDataAsync()
        {
            throw new NotImplementedException();
        }
    }
}
