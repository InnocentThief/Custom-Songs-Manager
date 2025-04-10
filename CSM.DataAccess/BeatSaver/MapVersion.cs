using System.Text.Json.Serialization;

namespace CSM.DataAccess.BeatSaver
{
    internal class MapVersion
    {
        [JsonPropertyName("coverURL")]
        public string CoverURL { get; set; } = string.Empty;

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("diffs")]
        public List<MapDifficulty> Diffs { get; set; } = [];

        [JsonPropertyName("downloadURL")]
        public string DownloadURL { get; set; } = string.Empty;

        [JsonPropertyName("feedback")]
        public string Feedback { get; set; } = string.Empty;

        [JsonPropertyName("hash")]
        public string Hash { get; set; } = string.Empty;

        [JsonPropertyName("key")]
        public string Key { get; set; } = string.Empty;

        [JsonPropertyName("previewURL")]
        public string PreviewURL { get; set; } = string.Empty;

        [JsonPropertyName("sageScore")]
        public short SageScore { get; set; }

        [JsonPropertyName("scheduledAt")]
        public DateTime ScheduledAt { get; set; }

        [JsonPropertyName("state")]
        public State State { get; set; }

        [JsonPropertyName("testplayAt")]
        public DateTime TestplayAt { get; set; }

        // Testplays (if needed)
    }
}
