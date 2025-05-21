using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.ViewModels.Controls.BeatLeader;

namespace CSM.UiLogic.ViewModels.Workspaces
{
    internal sealed class BeatLeaderWorkspaceViewModel(IServiceLocator serviceLocator) : WorkspaceViewModel(serviceLocator)
    {
        public override string Title => "Custom Songs Manager - BeatLeader";

        public BeatLeaderControlViewModel BeatLeaderControl { get; } = new BeatLeaderControlViewModel(serviceLocator);

        public override async Task ActivateAsync(bool refresh)
        {
            await BeatLeaderControl.LoadAsync(refresh);
        }
    }
}
