using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.ViewModels.Controls.ScoreSaber;

namespace CSM.UiLogic.ViewModels.Workspaces
{
    internal sealed class ScoreSaberWorkspaceViewModel : WorkspaceViewModel
    {
        public override string Title => "Custom Songs Manager - ScoreSaber";

        public ScoreSaberControlViewModel ScoreSaberControl { get; }

        public ScoreSaberWorkspaceViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
            ScoreSaberControl = new ScoreSaberControlViewModel(serviceLocator);
        }

        public override async Task ActivateAsync(bool refresh)
        {
            await ScoreSaberControl.LoadAsync(refresh);
        }
    }
}
