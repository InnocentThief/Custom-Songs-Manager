using CSM.DataAccess.BeatLeader;
using CSM.Framework.ServiceLocation;

namespace CSM.UiLogic.ViewModels.Common.Leaderboard
{
    internal sealed class BeatLeaderScoreViewModel : BaseScoreViewModel
    {
        private Score score;

        public int Rank => score.Rank;

        public string SongName => score.Leaderboard.Song.Name;

        public BeatLeaderScoreViewModel(IServiceLocator serviceLocator, Score score) : base(serviceLocator)
        {
            this.score = score;
        }
    }
}
