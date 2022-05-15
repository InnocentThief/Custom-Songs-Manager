using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Online
{
    /// <summary>
    /// Represents the BeatMap stats (as used in BeatSaver).
    /// </summary>
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