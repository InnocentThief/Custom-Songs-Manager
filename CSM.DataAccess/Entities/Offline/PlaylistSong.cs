using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Offline
{
    /// <summary>
    /// Represents a song inside the playlist.
    /// </summary>
    public class PlaylistSong
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonIgnore]
        public int BsrKeyHex
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Key)) return 0;
                return int.Parse(Key, System.Globalization.NumberStyles.HexNumber);
            }
        }

        [JsonPropertyName("songName")]
        public string SongName { get; set; }

        [JsonPropertyName("levelAuthorName")]
        public string LevelAuthorName { get; set; }

        [JsonPropertyName("hash")]
        public string Hash { get; set; }

        [JsonPropertyName("levelid")]
        public string Levelid { get; set; }

        [JsonPropertyName("difficulties")]
        public List<PlaylistSongDifficulty> Difficulties { get; set; }
    }
}