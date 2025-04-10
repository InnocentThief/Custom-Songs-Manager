using System.Text.Json.Serialization;

namespace CSM.DataAccess.Playlists
{
    internal class PlaylistCustomData
    {
        [JsonPropertyName("syncURL")]
        public string SyncURL { get; set; } = string.Empty;

        [JsonPropertyName("owner")]
        public string? Owner { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("hash")]
        public string? Hash { get; set; }

        [JsonPropertyName("shared")]
        public bool? Shared { get; set; }
    }
}
