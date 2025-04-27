using System.Text.Json.Serialization;

namespace CSM.DataAccess.BeatSaver
{
    internal class SearchInfo
    {
        [JsonPropertyName("pages")]
        public int Pages { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }
}
