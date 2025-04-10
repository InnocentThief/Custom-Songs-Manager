using System.Text.Json.Serialization;

namespace CSM.DataAccess.Playlists
{
  internal  class Difficulty
    {
        [JsonPropertyName("characteristic")]
        public string Characteristic { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("stars")]
        public int? Stars { get; set; }

        [JsonPropertyName("label")]
        public string? Label { get; set; }
    }
}
