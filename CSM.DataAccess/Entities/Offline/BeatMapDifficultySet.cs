using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Offline
{
    public class BeatMapDifficultySet
    {
        [JsonPropertyName("_beatmapCharacteristicName")]
        public string BeatmapCharacteristicName { get; set; }

        [JsonPropertyName("_difficultyBeatmaps")]
        public List<BeatMapDifficulty> Difficulties { get; set; }
    }
}