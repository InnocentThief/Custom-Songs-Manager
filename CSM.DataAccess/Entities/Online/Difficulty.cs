using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Online
{
    /// <summary>
    /// Represents a BeatMap difficulty (as used in BeatSaver).
    /// </summary>
    public class Difficulty
    {
        [JsonPropertyName("njs")]
        public decimal NoteJumpMovementSpeed { get; set; }

        [JsonPropertyName("offset")]
        public decimal NoteJumpStartBeatOffset { get; set; }

        [JsonPropertyName("nps")]
        public decimal Nps { get; set; }

        [JsonPropertyName("characteristic")]
        public string Characteristic { get; set; }

        [JsonPropertyName("difficulty")]
        public string Diff { get; set; }

        [JsonPropertyName("chroma")]
        public bool Chroma { get; set; }

        [JsonPropertyName("ne")]
        public bool Noodle { get; set; }

        [JsonPropertyName("me")]
        public bool MappingExtension { get; set; }

        [JsonPropertyName("stars")]
        public decimal Stars { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }
    }
}