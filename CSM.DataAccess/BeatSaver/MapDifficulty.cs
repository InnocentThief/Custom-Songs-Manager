using System.Text.Json.Serialization;
using CSM.DataAccess.Common;

namespace CSM.DataAccess.BeatSaver
{
    internal class MapDifficulty
    {
        [JsonPropertyName("blStars")]
        public decimal BlStars { get; set; }

        [JsonPropertyName("bombs")]
        public int Bombs { get; set; }

        [JsonPropertyName("characteristic")]
        public Characteristic Characteristic { get; set; }

        [JsonPropertyName("chroma")]
        public bool Chroma { get; set; }

        [JsonPropertyName("cinema")]
        public bool Cinema { get; set; }

        [JsonPropertyName("difficulty")]
        public Difficulty Difficulty { get; set; }

        [JsonPropertyName("environment")]
        public Environment Environment { get; set; }

        [JsonPropertyName("events")]
        public int Events { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; } = string.Empty;

        [JsonPropertyName("length")]
        public double Lenght { get; set; }

        [JsonPropertyName("maxScore")]
        public int MaxScore { get; set; }

        [JsonPropertyName("me")]
        public bool Me { get; set; }

        [JsonPropertyName("ne")]
        public bool Ne { get; set; }

        [JsonPropertyName("njs")]
        public decimal Njs { get; set; }

        [JsonPropertyName("notes")]
        public int Notes { get; set; }

        [JsonPropertyName("nps")]
        public double Nps { get; set; }

        [JsonPropertyName("obstacles")]
        public int Obstacles { get; set; }

        // Offset (if needed)

        // ParitySummary (if needed)

        [JsonPropertyName("seconds")]
        public double Seconds { get; set; }

        [JsonPropertyName("stars")]
        public decimal Stars { get; set; }

        // Vivify (if needed)
    }
}
