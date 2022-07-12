using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Online.ScoreSaber
{
    public class ScoreStats
    {
        [JsonPropertyName("totalScore")]
        public int TotalScore { get; set; }

        [JsonPropertyName("totalRankedScore")]
        public int TotalRankedScore { get; set; }

        [JsonPropertyName("averageRankedAccuracy")]
        public decimal AverageRankedAccuracy { get; set; }

        [JsonPropertyName("totalPlayCount")]
        public int TotalPlayCount { get; set; }

        [JsonPropertyName("rankedPlayCount")]
        public int RankedPlayCount { get; set; }

        [JsonPropertyName("replaysWatched")]
        public int ReplaysWatched { get; set; }
    }
}