using CSM.DataAccess.BeatLeader;
using CSM.Framework.ServiceLocation;

namespace CSM.UiLogic.ViewModels.Common.Leaderboard
{
    internal sealed class BeatLeaderPlayerViewModel : BasePlayerViewModel
    {
        private readonly Player player;

        public override string Name => player.Name;

        public override string Avatar => player.Avatar;

        public override double PP => Math.Round(player.Pp, 2);

        public override int Rank => player.Rank;

        public override string Country => player.Country;

        public override int CountryRank => player.CountryRank;

        public string HomeClan => player.ClanOrder.Split(',').FirstOrDefault() ?? string.Empty;

        public string HomeClanColor
        {
            get
            {
                var homeClan = player.ClanOrder.Split(',').FirstOrDefault() ?? string.Empty;
                var clanData = player.Clans.FirstOrDefault(c => c.Tag == homeClan);
                if (clanData == null)
                    return string.Empty;
                return clanData.Color;
            }
        }

        public List<Clan> Clans
        {
            get
            {
                var homeClan = player.ClanOrder.Split(',').FirstOrDefault() ?? string.Empty;

                var clanList = new List<Clan>();
                var clanOrder = player.ClanOrder.Split(',');
                foreach (var clan in clanOrder)
                {
                    var clanData = player.Clans.FirstOrDefault(c => c.Tag == clan);
                    if (clanData == null)
                        continue;
                    if (clanData.Tag != homeClan)
                        clanList.Add(clanData);
                }
                return clanList;
            }
        }

        public List<StatsViewModel> ScoreStats
        {
            get
            {
                var scoreStats = new List<StatsViewModel>();
                var profileAppearance = player.ProfileSettings.ProfileAppearance.Split(',');

                if (profileAppearance.Contains("totalPlayCount"))
                {
                    scoreStats.Add(new StatsViewModel($"Total play count", player.ScoreStats.TotalPlayCount.ToString()));
                }

                if (profileAppearance.Contains("totalScore"))
                {
                    scoreStats.Add(new StatsViewModel("Total score", player.ScoreStats.TotalScore.ToString()));
                }

                if (profileAppearance.Contains("rankedPlayCount"))
                {
                    scoreStats.Add(new StatsViewModel($"Ranked play count", player.ScoreStats.RankedPlayCount.ToString()));
                }

                if (profileAppearance.Contains("totalRankedScore"))
                {
                    scoreStats.Add(new StatsViewModel("Total ranked score", player.ScoreStats.TotalRankedScore.ToString()));
                }

                if (profileAppearance.Contains("topPp"))
                {
                    scoreStats.Add(new StatsViewModel($"Top pp", $"{Math.Round(player.ScoreStats.TopPp, 2)}pp"));
                }

                if (profileAppearance.Contains("topAccuracy"))
                {
                    scoreStats.Add(new StatsViewModel("Best acc", $"{Math.Round(player.ScoreStats.TopAccuracy, 2)}%"));
                }

                if (profileAppearance.Contains("topRankedAccuracy"))
                {
                    scoreStats.Add(new StatsViewModel("Best ranked acc", $"{Math.Round(player.ScoreStats.TopRankedAccuracy, 2)}%"));
                }

                if (profileAppearance.Contains("averageAccuracy"))
                {
                    scoreStats.Add(new StatsViewModel("Average acc", $"{Math.Round(player.ScoreStats.AverageAccuracy, 2)}%"));
                }

                if (profileAppearance.Contains("medianAccuracy"))
                {
                    scoreStats.Add(new StatsViewModel("Median acc", $"{Math.Round(player.ScoreStats.MedianAccuracy, 2)}%"));
                }

                if (profileAppearance.Contains("averageRankedAccuracy"))
                {
                    scoreStats.Add(new StatsViewModel("Average ranked acc", $"{Math.Round(player.ScoreStats.AverageRankedAccuracy, 2)}%"));
                }

                if (profileAppearance.Contains("averageWeightedRankedAccuracy"))
                {
                    scoreStats.Add(new StatsViewModel("Weighted ranked acc", $"{Math.Round(player.ScoreStats.AverageWeightedRankedAccuracy, 2)}%"));
                }

                if (profileAppearance.Contains("medianRankedAccuracy"))
                {
                    scoreStats.Add(new StatsViewModel("Median ranked acc", $"{Math.Round(player.ScoreStats.MedianRankedAccuracy, 2)}%"));
                }

                if (profileAppearance.Contains("averageWeightedRankedRank"))
                {
                    scoreStats.Add(new StatsViewModel("Weighted average rank", $"#{Math.Round(player.ScoreStats.AverageWeightedRankedRank, 2)}"));
                }

                if (profileAppearance.Contains("peakRank"))
                {
                    scoreStats.Add(new StatsViewModel($"Peak rank", player.ScoreStats.PeakRank.ToString()));
                }

                if (profileAppearance.Contains("averageRank"))
                {
                    scoreStats.Add(new StatsViewModel($"Average rank", $"#{Math.Round(player.ScoreStats.AverageRank, 2)}"));
                }

                if (profileAppearance.Contains("top1Count"))
                {
                    scoreStats.Add(new StatsViewModel($"#1's", player.ScoreStats.Top1Count.ToString()));
                }

                if (profileAppearance.Contains("topPercentile"))
                {
                    scoreStats.Add(new StatsViewModel("Global", $"Top {Math.Round(player.ScoreStats.TopPercentile * 100, 1)}% of players"));
                }

                if (profileAppearance.Contains("countryTopPercentile"))
                {
                    scoreStats.Add(new StatsViewModel("Country", $"Top {Math.Round(player.ScoreStats.CountryTopPercentile * 100, 1)}% of players"));
                }



                return scoreStats;
            }
        }

        public List<StatsViewModel> PlaysStats
        {
            get
            {
                var playsStats = new List<StatsViewModel>();
                var profileAppearance = player.ProfileSettings.ProfileAppearance.Split(',');

                if (profileAppearance.Contains("sspPlays"))
                {
                    playsStats.Add(new StatsViewModel($"SS+", player.ScoreStats.SspPlays.ToString()));
                }

                if (profileAppearance.Contains("ssPlays"))
                {
                    playsStats.Add(new StatsViewModel($"SS", player.ScoreStats.SsPlays.ToString()));
                }

                if (profileAppearance.Contains("spPlays"))
                {
                    playsStats.Add(new StatsViewModel($"S+", player.ScoreStats.SpPlays.ToString()));
                }

                if (profileAppearance.Contains("sPlays"))
                {
                    playsStats.Add(new StatsViewModel($"S", player.ScoreStats.SPlays.ToString()));
                }

                if (profileAppearance.Contains("aPlays"))
                {
                    playsStats.Add(new StatsViewModel($"A", player.ScoreStats.APlays.ToString()));
                }

                return playsStats;
            }
        }

        public BeatLeaderPlayerViewModel(IServiceLocator serviceLocator, Player player) : base(serviceLocator)
        {
            this.player = player;
        }
    }
}
