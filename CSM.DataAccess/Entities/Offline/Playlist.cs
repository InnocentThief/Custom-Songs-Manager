using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CSM.DataAccess.Entities.Offline
{
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