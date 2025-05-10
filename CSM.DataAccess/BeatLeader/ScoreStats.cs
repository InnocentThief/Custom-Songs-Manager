namespace CSM.DataAccess.BeatLeader
{
    internal class ScoreStats
    {
        public int Id { get; set; }
        public long TotalScore { get; set; }
        public long TotalUnrankedScore { get; set; }
        public long TotalRankedScore { get; set; }
        public long FirstScoreTime { get; set; }
        public long FirstUnrankedScoreTime { get; set; }
        public long FirstRankedScoreTime { get; set; }
        public long LastScoreTime { get; set; }
        public long LastUnrankedScoreTime { get; set; }
        public long LastRankedScoreTime { get; set; }
        public double AverageRankedAccuracy { get; set; }
        public double AverageWeightedRankedAccuracy { get; set; }
        public double AverageUnrankedAccuracy { get; set; }
        public double AverageAccuracy { get; set; }
        public double MedianRankedAccuracy { get; set; }
        public double MedianAccuracy { get; set; }
        public double TopRankedAccuracy { get; set; }
        public double TopUnrankedAccuracy { get; set; }
        public double TopAccuracy { get; set; }
        public double TopPp { get; set; }
        public double TopBonusPP { get; set; }
        public double TopPassPP { get; set; }
        public double TopAccPP { get; set; }
        public double TopTechPP { get; set; }
        public int PeakRank { get; set; }
        public int RankedMaxStreak { get; set; }
        public int UnrankedMaxStreak { get; set; }
        public int MaxStreak { get; set; }
        public double AverageLeftTiming { get; set; }
        public double AverageRightTiming { get; set; }
        public int RankedPlayCount { get; set; }
        public int UnrankedPlayCount { get; set; }
        public int TotalPlayCount { get; set; }
        public int RankedImprovementsCount { get; set; }
        public int UnrankedImprovementsCount { get; set; }
        public int TotalImprovementsCount { get; set; }
        public int RankedTop1Count { get; set; }
        public int UnrankedTop1Count { get; set; }
        public int Top1Count { get; set; }
        public int RankedTop1Score { get; set; }
        public int UnrankedTop1Score { get; set; }
        public int Top1Score { get; set; }
        public double AverageRankedRank { get; set; }
        public double AverageWeightedRankedRank { get; set; }
        public double AverageUnrankedRank { get; set; }
        public double AverageRank { get; set; }
        public int SspPlays { get; set; }
        public int SsPlays { get; set; }
        public int SpPlays { get; set; }
        public int SPlays { get; set; }
        public int APlays { get; set; }
        public string TopPlatform { get; set; } = string.Empty;
        public int TopHMD { get; set; }
        public string AllHMDs { get; set; } = string.Empty;
        public double TopPercentile { get; set; }
        public double CountryTopPercentile { get; set; }
        public int DailyImprovements { get; set; }
        public int AuthorizedReplayWatched { get; set; }
        public int AnonimusReplayWatched { get; set; }
        public int WatchedReplays { get; set; }
    }
}
