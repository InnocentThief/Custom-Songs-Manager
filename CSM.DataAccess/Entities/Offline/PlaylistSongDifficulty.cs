using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Offline
{
    /// <summary>
    /// Represents a difficulty for a song inside a playlist.
    /// </summary>
    public class PlaylistSongDifficulty
    {
        [JsonPropertyName("characteristic")]
        public string Characteristic { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonIgnore]
        public string NPS { get; set; }

        [JsonIgnore]
        public bool Noodle { get; set; }

        [JsonIgnore]
        public bool Chroma { get; set; }

        [JsonIgnore]
        public bool MappingExtensions { get; set; }
    }
}