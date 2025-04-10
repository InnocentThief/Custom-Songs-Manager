using System.Text.Json.Serialization;

namespace CSM.DataAccess.BeatSaver
{
    internal class MapDetailMetadata
    {
        [JsonPropertyName("bpm")]
        public decimal Bpm { get; set; }

        [JsonPropertyName("duration")]
        public int Duration { get; set; }

        [JsonPropertyName("levelAuthorName")]
        public string LevelAuthorName { get; set; } = string.Empty;

        [JsonPropertyName("songAuthorName")]
        public string SongAuthorName { get; set; } = string.Empty;

        [JsonPropertyName("songName")]
        public string SongName { get; set; } = string.Empty;

        [JsonPropertyName("songSubName")]
        public string SongSubName { get; set; } = string.Empty;
    }
}
