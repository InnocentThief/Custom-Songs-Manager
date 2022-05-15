using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Offline
{
    /// <summary>
    /// Represents a Playlist. The one on the disc in the playlist directory.
    /// </summary>
    public class Playlist
    {
        [JsonPropertyName("playlistTitle")]
        public string PlaylistTitle { get; set; }

        [JsonPropertyName("playlistAuthor")]
        public string PlaylistAuthor { get; set; }

        [JsonPropertyName("playlistDescription")]
        public string PlaylistDescription { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("songs")]
        public List<PlaylistSong> Songs { get; set; }
    }
}