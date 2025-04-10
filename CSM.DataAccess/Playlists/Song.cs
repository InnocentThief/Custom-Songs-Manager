using System.Text.Json.Serialization;

namespace CSM.DataAccess.Playlists
{
    internal class Song
    {
        [JsonPropertyName("key")]
        public string? Key { get; set; }

        [JsonPropertyName("songName")]
        public string SongName { get; set; } = string.Empty;

        [JsonPropertyName("levelAuthorName")]
        public string? LevelAuthorName { get; set; }

        [JsonPropertyName("hash")]
        public string Hash { get; set; } = string.Empty;

        [JsonPropertyName("levelid")]
        public string? LevelId { get; set; }

        [JsonPropertyName("customData")]
        public SongCustomData? CustomData { get; set; }

        [JsonPropertyName("difficulties")]
        public List<Difficulty>? Difficulties { get; set; }
    }
}
