using System.Text.Json.Serialization;

namespace CSM.DataAccess.CustomLevels
{
    internal class Song
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("subTitle")]
        public string SubTitle { get; set; } = string.Empty;

        [JsonPropertyName("author")]
        public string Author { get; set; } = string.Empty;
    }
}
