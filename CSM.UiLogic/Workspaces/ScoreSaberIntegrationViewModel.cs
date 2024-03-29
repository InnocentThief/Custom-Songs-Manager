﻿using CSM.Framework;
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

        public override void UnloadData()
        {
            base.UnloadData();
            if (ScoreSaber != null)
            {
                if (ScoreSaber.ScoreSaberSingle is ScoreSaberSinglePlayerAnalysisViewModel scoreSaber)
                {
                    if (scoreSaber.Player != null)
                    {
                        scoreSaber.Player.ShowedScores.Clear();
                    }
                }
            }
        }
    }
}
