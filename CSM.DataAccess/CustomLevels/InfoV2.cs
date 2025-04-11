using System.Text.Json.Serialization;

namespace CSM.DataAccess.CustomLevels
{
    internal class InfoV2 : InfoBase
    {
        [JsonPropertyName("_version")]
        public string Version { get; set; } = string.Empty;

        [JsonPropertyName("_songName")]
        public string SongName { get; set; } = string.Empty;

        [JsonPropertyName("_songSubName")]
        public string SongSubName { get; set; } = string.Empty;

        [JsonPropertyName("_songAuthorName")]
        public string SongAuthorName { get; set; } = string.Empty;

        [JsonPropertyName("_beatsPerMinute")]
        public double BeatsPerMinute { get; set; }

        [JsonPropertyName("_coverImageFilename")]
        public string CoverImageFilename { get; set; } = string.Empty;

        [JsonPropertyName("_difficultyBeatmapSets")]
        public List<DifficultyBeatmapSet> DifficultyBeatmapSets { get; set; } = [];
    }
}
