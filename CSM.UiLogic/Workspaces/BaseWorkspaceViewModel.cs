using CSM.Framework;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace CSM.UiLogic.Workspaces
{
    /// <summary>
    /// Base class for workspaces.
    /// </summary>
    public abstract class BaseWorkspaceViewModel : ObservableObject
    {
        public string Title { get; set; }

        public string IconGlyph { get; set; }

        public abstract WorkspaceType WorkspaceType { get; }

        /// <summary>
        /// Gets or sets whether a workspace is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Used to load the workspace data.
        /// </summary>
        public virtual void LoadData()
        {
            IsActive = true;
        }

        /// <summary>
        /// Used to unload the workspace data.
        /// </summary>
        public virtual void UnloadData()
        {
            IsActive = false;
        }
    }
}