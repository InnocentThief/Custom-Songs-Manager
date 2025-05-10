using System.Text.Json.Serialization;
using CSM.DataAccess.Common;

namespace CSM.DataAccess.CustomLevels
{
    internal class DifficultyBeatmapV4
    {
        [JsonPropertyName("characteristic")]
        public Characteristic Characteristic { get; set; }

        [JsonPropertyName("difficulty")]
        public Difficulty Difficulty { get; set; }

        public BeatmapAuthor? BeatmapAuthors { get; set; }
    }
}
