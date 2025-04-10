using System.Text.Json.Serialization;

namespace CSM.DataAccess.Playlists
{
    internal class Playlist
    {
        [JsonPropertyName("playlistTitle")]
        public string PlaylistTitle { get; set; } = string.Empty;

        [JsonPropertyName("playlistAuthor")]
        public string PlaylistAuthor { get; set; } = string.Empty;

        [JsonPropertyName("customData")]
        public PlaylistCustomData? CustomData { get; set; }

        [JsonPropertyName("playlistDescription")]
        public string? PlaylistDescription { get; set; }

        [JsonPropertyName("songs")]
        public List<Song> Songs { get; set; } = [];

        [JsonPropertyName("image")]
        public string Image { get; set; } = string.Empty;
    }
}
