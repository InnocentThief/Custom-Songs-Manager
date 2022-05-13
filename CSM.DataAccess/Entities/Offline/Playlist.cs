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
    }
}