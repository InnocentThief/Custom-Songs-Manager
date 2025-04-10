using System.Text.Json.Serialization;

namespace CSM.DataAccess.CustomLevels
{
    internal class InfoV4 : InfoBase
    {
        [JsonPropertyName("version")]
        public string Version { get; set; } = string.Empty;

        [JsonPropertyName("song")]
        public Song Song { get; set; } = new Song();

        [JsonPropertyName("audio")]
        public Audio Audio { get; set; } = new Audio();

        [JsonPropertyName("difficultyBeatmaps")]
        public List<DifficultyBeatmapV4> DifficultyBeatmaps { get; set; } = [];
    }
}
