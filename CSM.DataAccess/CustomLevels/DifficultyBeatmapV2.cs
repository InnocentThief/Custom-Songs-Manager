using System.Text.Json.Serialization;
using CSM.DataAccess.Common;

namespace CSM.DataAccess.CustomLevels
{
    internal class DifficultyBeatmapV2
    {
        [JsonPropertyName("_difficulty")]
        public Difficulty Difficulty { get; set; }
    }
}
