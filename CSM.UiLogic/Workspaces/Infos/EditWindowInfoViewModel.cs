using CSM.UiLogic.Wizards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.Infos
{
    public class EditWindowInfoViewModel : EditWindowBaseViewModel
    {
        public override int Height => 200;

        public override int Width => 400;

        public override string Title => "About Custom Songs Manager";
    }
}
