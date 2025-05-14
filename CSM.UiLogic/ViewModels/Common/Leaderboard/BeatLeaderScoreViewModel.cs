using CSM.DataAccess.BeatLeader;
using CSM.Framework.ServiceLocation;

namespace CSM.UiLogic.ViewModels.Common.Leaderboard
{
    internal sealed class BeatLeaderScoreViewModel : BaseScoreViewModel
    {
        private Score score;

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

        public BeatLeaderScoreViewModel(IServiceLocator serviceLocator, Score score) : base(serviceLocator)
        {
            this.score = score;
        }
    }
}
