using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Online
{
    public class Stats
    {
        [JsonPropertyName("upvotes")]
        public int Upvotes { get; set; }

        [JsonPropertyName("downvotes")]
        public int Downvotes { get; set; }

        [JsonPropertyName("score")]
        public decimal Score { get; set; }
    }
}