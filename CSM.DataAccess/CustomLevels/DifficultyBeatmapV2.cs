using CSM.DataAccess.Common;
using System.Text.Json.Serialization;

namespace CSM.DataAccess.CustomLevels
{
    internal class DifficultyBeatmapV2
    {
        [JsonPropertyName("_difficulty")]
        public Difficulty Difficulty { get; set; }
    }
}
