using CSM.DataAccess.Common;
using System.Text.Json.Serialization;

namespace CSM.DataAccess.CustomLevels
{
    internal class DifficultyBeatmapV4
    {
        [JsonPropertyName("characteristic")]
        public Characteristic Characteristic { get; set; }

        [JsonPropertyName("difficulty")]
        public Difficulty Difficulty { get; set; }
    }
}
