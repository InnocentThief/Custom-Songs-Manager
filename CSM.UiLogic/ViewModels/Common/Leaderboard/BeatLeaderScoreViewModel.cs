using CSM.DataAccess.BeatLeader;
using CSM.DataAccess.Common;
using CSM.Framework.ServiceLocation;

namespace CSM.UiLogic.ViewModels.Common.Leaderboard
{
    internal sealed class BeatLeaderScoreViewModel : BaseScoreViewModel
    {
        private readonly Score score;

        public override int Rank => score.Rank;
        public override string SongName => score.Leaderboard.Song.Name;
        public override bool FC => score.FullCombo;
        public override int BadCuts => score.BadCuts;
        public override int MissedNotes => score.MissedNotes;
        public override string Id => score.Leaderboard.Song.Id;
        public override string SubName => score.Leaderboard.Song.SubName;
        public override string Author => score.Leaderboard.Song.Author;
        public override string Mapper => score.Leaderboard.Song.Mapper;
        public override string CoverImage => score.Leaderboard.Song.CoverImage;
        public override double Accuracy => Math.Round(score.Accuracy * 100, 2);
        public string FcAccuracy => $"{Math.Round(score.FcAccuracy * 100, 2)}%";
        public override double PP => Math.Round(score.PP, 2);
        public override double WeightedPP => Math.Round(PP * score.Weight, 2);
        public override int Pauses => score.Pauses;
        public override string Modifiers => score.Modifiers.Replace(",", ", ");

        public override DateTime Date => DateTimeOffset.FromUnixTimeSeconds(score.Timepost).Date;

        public override Characteristic Characteristic => score.Leaderboard.Difficulty.ModeName;

        public override DataAccess.Common.Difficulty Difficulty => score.Leaderboard.Difficulty.DifficultyName;

        public BeatLeaderScoreViewModel(IServiceLocator serviceLocator, Score score) : base(serviceLocator)
        {
            this.score = score;
        }
    }
}
