using CSM.DataAccess.Common;
using System.Text.Json.Serialization;

namespace CSM.DataAccess.Playlists
{
    internal class Difficulty
    {
        [JsonPropertyName("characteristic")]
        public Characteristic Characteristic { get; set; }

        [JsonPropertyName("name")]
        public Common.Difficulty Name { get; set; }

        [JsonPropertyName("stars")]
        public int? Stars { get; set; }

        [JsonPropertyName("label")]
        public string? Label { get; set; }
    }
}
