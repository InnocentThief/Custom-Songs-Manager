using CSM.Framework;
using CSM.UiLogic.Workspaces.ScoreSaberIntegration;

namespace CSM.UiLogic.Workspaces
{
    internal class ScoreSaberIntegrationViewModel : BaseWorkspaceViewModel
    {
        public override WorkspaceType WorkspaceType => WorkspaceType.ScoreSaberIntegration;

        /// <summary>
        /// Gets the view model for the ScoreSaber area.
        /// </summary>
        public ScoreSaberViewModel ScoreSaber { get; }

        public ScoreSaberIntegrationViewModel()
        {
            ScoreSaber = new ScoreSaberViewModel();
        }
    }
}
