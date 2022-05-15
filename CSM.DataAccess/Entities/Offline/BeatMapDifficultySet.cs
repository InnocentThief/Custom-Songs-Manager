using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Offline
{
    /// <summary>
    /// Represents a difficulty set.
    /// </summary>
    public class BeatMapDifficultySet
    {
        [JsonPropertyName("_beatmapCharacteristicName")]
        public string BeatmapCharacteristicName { get; set; }

        [JsonPropertyName("_difficultyBeatmaps")]
        public List<BeatMapDifficulty> Difficulties { get; set; }
    }
}