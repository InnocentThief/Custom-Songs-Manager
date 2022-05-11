using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Online
{
    public class Metadata
    {
        [JsonPropertyName("bpm")]
        public decimal Bpm { get; set; }

        [JsonPropertyName("duration")]
        public decimal Duration { get; set; }
    }
}