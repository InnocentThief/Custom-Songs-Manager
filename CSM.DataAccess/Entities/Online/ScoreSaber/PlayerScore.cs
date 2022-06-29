using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Online.ScoreSaber
{
    public class PlayerScore
    {
        [JsonPropertyName("score")]
        public Score Score { get; set; }

        [JsonPropertyName("leaderboard")]
        public LeaderboardInfo Leaderboard { get; set; }
    }
}