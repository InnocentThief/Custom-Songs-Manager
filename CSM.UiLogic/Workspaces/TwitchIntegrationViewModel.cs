using CSM.Framework;
using CSM.UiLogic.Workspaces.TwitchIntegration;
using CSM.UiLogic.Workspaces.TwitchIntegration.ScoreSaberIntegration;

namespace CSM.UiLogic.Workspaces
{
    /// <summary>
    /// ViewModel for the Twitch Integration workspace.
    /// </summary>
    internal class TwitchIntegrationViewModel : BaseWorkspaceViewModel
    {

        public ScoreSaberViewModel ScoreSaber { get; }

        public TwitchViewModel Twitch { get; }

        /// <summary>
        /// Gets the workspace type.
        /// </summary>
        public override WorkspaceType WorkspaceType => WorkspaceType.TwitchIntegration;

        public TwitchIntegrationViewModel()
        {
            ScoreSaber = new ScoreSaberViewModel();
            Twitch = new TwitchViewModel();
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