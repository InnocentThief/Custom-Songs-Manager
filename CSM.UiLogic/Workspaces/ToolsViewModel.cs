using CSM.Framework;

namespace CSM.UiLogic.Workspaces
{
    /// <summary>
    /// ViewModel for the Tools workspace.
    /// </summary>
    internal class ToolsViewModel : BaseWorkspaceViewModel
    {
        /// <summary>
        /// Gets the workspace type.
        /// </summary>
        public override WorkspaceType WorkspaceType => WorkspaceType.Tools;

        /// <summary>
        /// Used to load the workspace data.
        /// </summary>
        public override void LoadData()
        {
            
        }

        /// <summary>
        /// Used to unload the workspace data.
        /// </summary>
        public override void UnloadData()
        {

        }
    }
}