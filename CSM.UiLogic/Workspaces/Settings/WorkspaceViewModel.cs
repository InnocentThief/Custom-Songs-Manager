using CSM.Framework;

namespace CSM.UiLogic.Workspaces.Settings
{
    /// <summary>
    /// Represents one workspace (used to select the default workspace).
    /// </summary>
    public class WorkspaceViewModel
    {
        /// <summary>
        /// Gets the name of the workspace.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type of the workspace.
        /// </summary>
        public WorkspaceType Type { get; }

        /// <summary>
        /// Initializes a new <see cref="WorkspaceViewModel"/>.
        /// </summary>
        /// <param name="name">The name of the workspace.</param>
        /// <param name="type">The type of the workspace.</param>
        public WorkspaceViewModel(string name, WorkspaceType type)
        {
            Name = name;
            Type = type;
        }
    }
}