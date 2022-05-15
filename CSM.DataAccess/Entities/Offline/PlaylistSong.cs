using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Offline
{
    /// <summary>
    /// Represents a song inside the playlist.
    /// </summary>
    public class PlaylistSong
    {
        [JsonPropertyName("songName")]
        public string SongName { get; set; }
    }
}