using System.Text.Json.Serialization;

namespace CSM.DataAccess.CustomLevels
{
    internal class BeatmapAuthor
    {
        [JsonPropertyName("mappers")]
        public List<string> Mappers { get; set; } = [];

        [JsonPropertyName("lighters")]
        public List<string> Lighters { get; set; } = [];
    }
}
