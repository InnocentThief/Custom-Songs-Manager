using CSM.Framework;
using CSM.UiLogic.Workspaces.Tools.CleanupCustomLevels;

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
        /// Gets the view model for the custom levels tools.
        /// </summary>
        public CleanupCustomLevelsViewModel CustomLevels { get; }

        /// <summary>
        /// Initializes a new <see cref="ToolsViewModel"/>.
        /// </summary>
        public ToolsViewModel()
        {
            CustomLevels = new CleanupCustomLevelsViewModel();
        }

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