using CSM.Framework;

namespace CSM.UiLogic.Workspaces.Settings
{
    /// <summary>
    /// Represents one workspace (used to select the default workspace).
    /// </summary>
    public class WorkspaceViewModel
    {
        public string Name { get; }

        public WorkspaceType Type { get; }

        public WorkspaceViewModel(string name, WorkspaceType type)
        {
            Name = name;
            Type = type;
        }
    }
}