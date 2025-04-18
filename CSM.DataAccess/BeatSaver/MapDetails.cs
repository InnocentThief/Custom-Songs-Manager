using System.Text.Json.Serialization;

namespace CSM.DataAccess.BeatSaver
{
    internal class MapDetails
    {
        [JsonPropertyName("docs")]
        public List<MapDetail> Docs { get; set; } = [];
    }
}
