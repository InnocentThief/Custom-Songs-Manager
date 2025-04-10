using CSM.DataAccess.Common;
using System.Text.Json.Serialization;

namespace CSM.DataAccess.Playlists
{
  internal  class Difficulty
    {
        [JsonPropertyName("characteristic")]
        public Characteristic Characteristic { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("stars")]
        public int? Stars { get; set; }

        [JsonPropertyName("label")]
        public string? Label { get; set; }
    }
}
