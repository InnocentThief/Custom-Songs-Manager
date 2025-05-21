using CSM.DataAccess.ScoreSaber;
using CSM.Framework.ServiceLocation;

namespace CSM.UiLogic.ViewModels.Common.Leaderboard
{
    internal sealed class ScoreSaberPlayerViewModel(IServiceLocator serviceLocator, Player player) : BasePlayerViewModel(serviceLocator)
    {
        private readonly Player player = player;

        public override string Id => player.Id;

        public override string Name => player.Name;

        public override string Avatar => player.ProfilePicture;

        public override string PP => player.PP.ToString("N2");

        public override string Rank => player.Rank.ToString("N0");

        public override string Country => player.Country;

        public override string CountryRank => player.CountryRank.ToString("N0");

        public List<StatsViewModel> ScoreStats
        {
            get
            {
                var scoreStats = new List<StatsViewModel>
                {
                    new("Total score", player.ScoreStats.TotalScore.ToString("N0")),
                    new("Total ranked score", player.ScoreStats.TotalRankedScore.ToString("N0")),
                    new("Average ranked accuracy", $"{Math.Round( player.ScoreStats.AverageRankedAccuracy, 2)}%"),
                    new("Total play count", player.ScoreStats.TotalPlayCount.ToString("N0")),
                    new("Ranked play count", player.ScoreStats.RankedPlayCount.ToString("N0")),
                };

                return scoreStats;
            }
        }
    }
}
