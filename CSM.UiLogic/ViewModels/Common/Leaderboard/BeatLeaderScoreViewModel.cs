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

        public BeatLeaderScoreViewModel(IServiceLocator serviceLocator, Score score) : base(serviceLocator)
        {
            this.score = score;
        }
    }
}
