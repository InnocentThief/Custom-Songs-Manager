using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Online
{
    /// <summary>
    /// Represents a BeatMap characteristic (as used in BeatSaver).
    /// </summary>
    public class Version
    {
        [JsonPropertyName("hash")]
        public string Hash { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("diffs")]
        public List<Difficulty> Difficulties { get; set; }

        [JsonPropertyName("coverURL")]
        public string CoverUrl { get; set; }
    }
}