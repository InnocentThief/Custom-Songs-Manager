using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces
{
    public abstract class BaseWorkspaceViewModel: ObservableObject
    {
        public string Title { get; set; }

        public string IconGlyph { get; set; }

        public abstract WorkspaceType WorkspaceType { get; }

        public abstract void LoadData();

        public abstract void UnloadData();
    }
}
