namespace CSM.DataAccess.ScoreSaber
{
    internal class ScoreStats
    {
        public long TotalScore { get; set; }
        public long TotalRankedScore { get; set; }
        public double AverageRankedAccuracy { get; set; }
        public int TotalPlayCount { get; set; }
        public int RankedPlayCount { get; set; }
        public int ReplaysWatched { get; set; }
    }
}
