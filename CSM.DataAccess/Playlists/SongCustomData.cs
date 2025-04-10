using System.Text.Json.Serialization;

namespace CSM.DataAccess.Playlists
{
    internal class SongCustomData
    {
        [JsonPropertyName("qualifiedTime")]
        public long QualifiedTime { get; set; }
    }
}
