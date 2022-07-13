using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Online.ScoreSaber
{
    public class PlayerScoreCollection
    {
        [JsonPropertyName("playerScores")]
        public List<PlayerScore> PlayerScores { get; set; }

        [JsonPropertyName("metadata")]
        public Metadata Metadata { get; set; }
    }
}