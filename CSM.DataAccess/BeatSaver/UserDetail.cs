using System.Text.Json.Serialization;

namespace CSM.DataAccess.BeatSaver
{
    internal class UserDetail
    {
        [JsonPropertyName("hash")]
        public string Hash { get; set; } = string.Empty;

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}
