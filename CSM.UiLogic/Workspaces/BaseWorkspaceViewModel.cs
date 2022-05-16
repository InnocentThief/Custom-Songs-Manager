using CSM.Framework;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace CSM.UiLogic.Workspaces
{
    /// <summary>
    /// Base class for workspaces.
    /// </summary>
    public abstract class BaseWorkspaceViewModel : ObservableObject
    {
        /// <summary>
        /// Gets or sets the title of the workspace.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the image of the workspace.
        /// </summary>
        public string IconGlyph { get; set; }

        /// <summary>
        /// Gets the workspace type.
        /// </summary>
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