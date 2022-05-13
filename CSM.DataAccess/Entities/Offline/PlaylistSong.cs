using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CSM.DataAccess.Entities.Offline
{
    public class PlaylistSong
    {
        [JsonPropertyName("songName")]
        public string SongName { get; set; }
    }
}
