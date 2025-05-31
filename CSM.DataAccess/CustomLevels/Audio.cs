using System.Text.Json.Serialization;

namespace CSM.DataAccess.CustomLevels
{
    internal class Audio
    {
        [JsonPropertyName("songDuration")]
        public double Duration { get; set; }

        [JsonPropertyName("bpm")]
        public double Bpm { get; set; }
    }
}
