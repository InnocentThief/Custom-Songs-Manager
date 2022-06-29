using CSM.DataAccess.Entities.Online.ScoreSaber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.TwitchIntegration.ScoreSaberIntegration
{
    public class ScoreSaberPlayerViewModel
    {
        public string Id { get; set; }

        public string Name { get; }

        public string ProfilePicture { get; }

        public string Rank { get; }

        public string PP { get; }

        public ScoreSaberPlayerViewModel(Player player)
        {
            Name = "InnocentThief";
            ProfilePicture = "https://cdn.scoresaber.com/avatars/76561198319524592.jpg";
            Rank = "#6044";
            PP = "5742.13 pp";
        }
    }
}