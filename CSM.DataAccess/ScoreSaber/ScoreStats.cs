namespace CSM.DataAccess.ScoreSaber
{
    internal class ScoreStats
    {
        public int TotalScore { get; set; }
        public int TotalRankedScore { get; set; }
        public double AverageRankedAccuracy { get; set; }
        public int TotalPlayCount { get; set; }
        public int RankedPlayCount { get; set; }
        public int ReplaysWatched { get; set; }
    }
}
