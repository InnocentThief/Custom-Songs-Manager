using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Online.ScoreSaber
{
    public class LeaderboardInfo
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("songHash")]
        public string SongHash { get; set; }

        [JsonPropertyName("songName")]
        public string SongName { get; set; }

        [JsonPropertyName("songSubName")]
        public string SongSubName { get; set; }

        [JsonPropertyName("songAuthorName")]
        public string SongAuthorName { get; set; }

        [JsonPropertyName("levelAuthorName")]
        public string LevelAuthorName { get; set; }

        [JsonPropertyName("difficulty")]
        public Difficulty Difficulty { get; set; }

        [JsonPropertyName("maxScore")]
        public decimal MaxScore { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonPropertyName("rankedDate")]
        public DateTime? RankedDate { get; set; }

        [JsonPropertyName("qualifiedDate")]
        public DateTime? QualifiedDate { get; set; }

        [JsonPropertyName("lovedDate")]
        public DateTime? LovedDate { get; set; }

        [JsonPropertyName("ranked")]
        public bool Ranked { get; set; }

        [JsonPropertyName("qualified")]
        public bool Qualified { get; set; }

        [JsonPropertyName("loved")]
        public bool Loved { get; set; }

        [JsonPropertyName("maxPP")]
        public decimal MaxPP { get; set; }

        [JsonPropertyName("stars")]
        public decimal Stars { get; set; }

        [JsonPropertyName("positiveModifiers")]
        public bool PositiveModifiers { get; set; }

        [JsonPropertyName("plays")]
        public int Plays { get; set; }

        [JsonPropertyName("dailyPlays")]
        public int DailyPlays { get; set; }

        [JsonPropertyName("coverImage")]
        public string CoverImage { get; set; }

        [JsonPropertyName("playerScore")]
        public Score PlayerScore { get; set; }

        [JsonPropertyName("difficulties")]
        public ICollection<Difficulty> Difficulties { get; set; }
    }
}