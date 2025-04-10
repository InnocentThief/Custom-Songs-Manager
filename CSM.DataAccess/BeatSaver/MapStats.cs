using System.Text.Json.Serialization;

namespace CSM.DataAccess.BeatSaver
{
    internal class MapStats
    {
        [JsonPropertyName("downloads")]
        public int Downloads { get; set; }

        [JsonPropertyName("downvotes")]
        public int Downvotes { get; set; }

        [JsonPropertyName("plays")]
        public int Plays { get; set; }

        [JsonPropertyName("reviews")]
        public int Reviews { get; set; }

        [JsonPropertyName("score")]
        public decimal Score { get; set; }

        [JsonPropertyName("scoreOneDP")]
        public decimal ScoreOneDP { get; set; }

        [JsonPropertyName("sentiment")]
        public Sentiment Sentiment { get; set; }

        [JsonPropertyName("upvotes")]
        public int Upvotes { get; set; }
    }
}
