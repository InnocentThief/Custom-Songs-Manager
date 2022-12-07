using CSM.DataAccess.Entities.Online.ScoreSaber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.ScoreSaberIntegration
{
    public class ScoreSaberSongViewModel
    {
        private LeaderboardInfo leaderboardInfo;

        public string SongHash => leaderboardInfo.SongHash;

        public string SongName => leaderboardInfo.SongName;

        public string SongAuthorName => leaderboardInfo.SongAuthorName;

        public string LevelAuthorName => leaderboardInfo.LevelAuthorName;

        public string CoverImage => leaderboardInfo.CoverImage;

        public string SongColumnText => $"{leaderboardInfo.SongName} {leaderboardInfo.SongAuthorName} {leaderboardInfo.LevelAuthorName}";

        public string Player1Name => "Player 1";

        public string Player1ACC => "99.9";


        public string Player1ProfilePicture => "https://cdn.scoresaber.com/avatars/76561198319524592.jpg";

        public ScoreSaberSongViewModel(LeaderboardInfo leaderboardInfo)
        {
            this.leaderboardInfo = leaderboardInfo;
        }

        public void AddPlayer(int playerIndex, decimal pp)
        {

        }

        public void RemovePlayer()
        {

        }
    }
}