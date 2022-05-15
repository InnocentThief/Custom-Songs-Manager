using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Online
{
    /// <summary>
    /// Represents the BeatMap metadata (as used in BeatSaver).
    /// </summary>
    public class Metadata
    {
        [JsonPropertyName("bpm")]
        public decimal Bpm { get; set; }

        [JsonPropertyName("duration")]
        public decimal Duration { get; set; }

        [JsonPropertyName("songName")]
        public string SongName { get; set; }

        [JsonPropertyName("songSubName")]
        public string SongSubName { get; set; }

        [JsonPropertyName("songAuthorName")]
        public string SongAuthorName { get; set; }

        [JsonPropertyName("levelAuthorName")]
        public string LevelAuthorName { get; set; }
    }
}