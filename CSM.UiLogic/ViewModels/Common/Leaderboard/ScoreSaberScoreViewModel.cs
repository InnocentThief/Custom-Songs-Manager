using CSM.DataAccess.Common;
using CSM.DataAccess.ScoreSaber;
using CSM.Framework.ServiceLocation;

namespace CSM.UiLogic.ViewModels.Common.Leaderboard
{
    internal sealed class ScoreSaberScoreViewModel(IServiceLocator serviceLocator, PlayerScore score) : BaseScoreViewModel(serviceLocator)
    {
        private readonly PlayerScore score = score;

        public override int Rank => score.Score.Rank;
        public override string SongName => score.Leaderboard.SongName;
        public override bool FC => score.Score.FullCombo;
        public override int BadCuts => score.Score.BadCuts;
        public override int MissedNotes => score.Score.MissedNotes;
        public override string Id => score.Leaderboard.Id.ToString();
        public override string SubName => score.Leaderboard.SongSubName;
        public override string Author => score.Leaderboard.SongAuthorName;
        public override string Mapper => score.Leaderboard.LevelAuthorName;
        public override string CoverImage => score.Leaderboard.CoverImage;
        public override double Accuracy
        {
            get
            {
                if (score.Leaderboard.MaxScore == 0)
                {
                    return 0;
                }
                return Math.Round(((double)score.Score.BaseScore / score.Leaderboard.MaxScore) * 100, 2);
            }
        }
        public override double PP => Math.Round(score.Score.PP, 2);
        public override double WeightedPP => Math.Round(PP * score.Score.Weight, 2);
        public override int Pauses => 0; // ScoreSaber does not provide pause information
        public override string Modifiers => score.Score.Modifiers.Replace(",", ", ");
        public override DateTime Date => score.Score.TimeSet;

        public override Characteristic Characteristic => score.Leaderboard.Difficulty.Characteristic;

        public override Difficulty Difficulty => (Difficulty)score.Leaderboard.Difficulty.Difficulty;
    }
}
