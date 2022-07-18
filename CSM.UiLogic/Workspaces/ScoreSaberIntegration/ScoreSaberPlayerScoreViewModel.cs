using CSM.DataAccess.Entities.Online.ScoreSaber;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.ScoreSaberIntegration
{
    public class ScoreSaberPlayerScoreViewModel
    {
        private PlayerScore playerScore;

        public int Rank => playerScore.Score.Rank;

        public DateTime TimeSet => DateTime.Parse(playerScore.Score.TimeSet, CultureInfo.InvariantCulture);

        public string TimeSetText
        {
            get
            {
                var timeSpan = DateTime.Now - DateTime.Parse(playerScore.Score.TimeSet, CultureInfo.InvariantCulture);
                if (timeSpan.Days > 365)
                {
                    return $"{timeSpan.Days / 365}y ago";
                }
                else if (timeSpan.Days > 30)
                {
                    return $"{timeSpan.Days / 30}mo ago";
                }
                else
                {
                    return $"{timeSpan.Days}d ago";
                }
            }
        }

        public int Difficulty => playerScore.Leaderboard.Difficulty.Diff;

        public string CoverImage => playerScore.Leaderboard.CoverImage;

        public string SongColumnText => $"{playerScore.Leaderboard.SongName} {playerScore.Leaderboard.SongAuthorName} {playerScore.Leaderboard.LevelAuthorName}";

        public string SongName => playerScore.Leaderboard.SongName;

        public string SongAuthorName => playerScore.Leaderboard.SongAuthorName;

        public string LevelAuthorName => playerScore.Leaderboard.LevelAuthorName;

        public decimal Accuracy
        {
            get
            {
                if (playerScore.Leaderboard.MaxScore > 0)
                {
                    return Math.Round(playerScore.Score.BaseScore / playerScore.Leaderboard.MaxScore * 100, 2);
                }
                else
                {
                    return 0;
                }
            }
        }

        public bool FullCombo => playerScore.Score.FullCombo;

        public decimal PP => Math.Round(playerScore.Score.PP, 2);

        public decimal WeightPP => Math.Round(playerScore.Score.Weight * playerScore.Score.PP, 2);

        public int MissedNotes => playerScore.Score.MissedNotes;

        public int BadCuts => playerScore.Score.BadCuts;

        public string Stars => $"{playerScore.Leaderboard.Stars}*";

        public ScoreSaberPlayerScoreViewModel(PlayerScore playerScore)
        {
            this.playerScore = playerScore;
        }
    }
}