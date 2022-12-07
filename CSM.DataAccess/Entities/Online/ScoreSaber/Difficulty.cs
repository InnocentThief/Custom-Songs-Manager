using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Online.ScoreSaber
{
    public class Difficulty
    {
        [JsonPropertyName("leaderboardId")]
        public long LeaderboardId { get; set; }

        [JsonPropertyName("difficulty")]
        public int Diff { get; set; }

        [JsonPropertyName("gameMode")]
        public string GameMode { get; set; }

        [JsonPropertyName("difficultyRaw")]
        public string DifficultyRaw { get; set; }
    }
}