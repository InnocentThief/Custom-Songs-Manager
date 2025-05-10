using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.ViewModels.Controls.BeatLeader;

namespace CSM.UiLogic.ViewModels.Workspaces
{
    internal sealed class BeatLeaderWorkspaceViewModel : WorkspaceViewModel
    {
        public override string Title => "Custom Songs Manager - BeatLeader";

        public BeatLeaderControlViewModel BeatLeaderControl { get; }

        public BeatLeaderWorkspaceViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
            BeatLeaderControl = new BeatLeaderControlViewModel(ServiceLocator);
        }

        public override async Task ActivateAsync(bool refresh)
        {
            await BeatLeaderControl.LoadAsync(refresh);
        }
    }
}
