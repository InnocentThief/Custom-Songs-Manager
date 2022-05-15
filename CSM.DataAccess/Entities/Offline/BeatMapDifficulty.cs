using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Offline
{
    /// <summary>
    /// Represents a beat map difficulty.
    /// </summary>
    public class BeatMapDifficulty
    {
        [JsonPropertyName("_difficulty")]
        public string Difficulty { get; set; }

        [JsonPropertyName("_difficultyRank")]
        public int DifficultyRank { get; set; }

        [JsonPropertyName("_beatmapFilename")]
        public string BeatmapFilename { get; set; }

        [JsonPropertyName("_noteJumpMovementSpeed")]
        public decimal NoteJumpMovementSpeed { get; set; }

        [JsonPropertyName("_noteJumpStartBeatOffset")]
        public decimal NoteJumpStartBeatOffset { get; set; }
    }
}