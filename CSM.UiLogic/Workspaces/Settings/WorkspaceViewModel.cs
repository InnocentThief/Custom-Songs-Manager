using CSM.Framework;

namespace CSM.UiLogic.Workspaces.Settings
{
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