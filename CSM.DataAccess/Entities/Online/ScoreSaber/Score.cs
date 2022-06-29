using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Online.ScoreSaber
{
    public class Score
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("rank")]
        public int Rank { get; set; }

        [JsonPropertyName("baseScore")]
        public int BaseScore { get; set; }

        [JsonPropertyName("modifiedScore")]
        public int ModifiedScore { get; set; }

        [JsonPropertyName("pp")]
        public decimal PP { get; set; }

        [JsonPropertyName("weight")]
        public decimal Weight { get; set; }

        [JsonPropertyName("modifiers")]
        public string Modifiers { get; set; }

        [JsonPropertyName("multiplier")]
        public decimal Multiplier { get; set; }

        [JsonPropertyName("badCuts")]
        public int BadCuts { get; set; }

        [JsonPropertyName("missedNotes")]
        public int MissedNotes { get; set; }

        [JsonPropertyName("maxCombo")]
        public int MaxCombo { get; set; }

        [JsonPropertyName("fullCombo")]
        public bool FullCombo { get; set; }

        [JsonPropertyName("hmd")]
        public int Hmd { get; set; }

        [JsonPropertyName("hasReplay")]
        public bool HasReplay { get; set; }

        [JsonPropertyNameAttribute("timeSet")]
        public string TimeSet { get; set; }
    }
}