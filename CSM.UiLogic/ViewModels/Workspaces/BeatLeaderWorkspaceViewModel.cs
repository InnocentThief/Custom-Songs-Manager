using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Workspaces
{
    internal sealed class BeatLeaderWorkspaceViewModel : WorkspaceViewModel
    {
        public override string Title => "Custom Songs Manager - BeatLeader";

        public BeatLeaderWorkspaceViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override async Task ActivateAsync(bool refresh)
        {
            await Task.CompletedTask;
        }
    }
}
