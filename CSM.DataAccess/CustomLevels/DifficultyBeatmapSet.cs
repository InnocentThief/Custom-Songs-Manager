using System.Text.Json.Serialization;

namespace CSM.DataAccess.CustomLevels
{
    internal class DifficultyBeatmapSet
    {
        [JsonPropertyName("_beatmapCharacteristicName")]
        public string BeatmapCharacteristicName { get; set; } = string.Empty;

        [JsonPropertyName("_difficultyBeatmaps")]
        public List<DifficultyBeatmapV2> DifficultyBeatmaps { get; set; } = [];
    }
}
