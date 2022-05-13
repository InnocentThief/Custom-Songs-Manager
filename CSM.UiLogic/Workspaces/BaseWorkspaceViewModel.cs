using CSM.Framework;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace CSM.UiLogic.Workspaces
{
    public abstract class BaseWorkspaceViewModel : ObservableObject
    {
        public string Title { get; set; }

        public string IconGlyph { get; set; }

        public abstract WorkspaceType WorkspaceType { get; }

        public bool IsActive { get; set; }

        public virtual void LoadData()
        {
            IsActive = true;
        }

        public virtual void UnloadData()
        {
            IsActive = false;
        }

    }
}