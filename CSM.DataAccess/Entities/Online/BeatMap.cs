using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Online
{
    /// <summary>
    /// Represents a BeatMap (as used in BeatSaver).
    /// </summary>
    public class BeatMap
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("uploader")]
        public Uploader Uploader { get; set; }

        [JsonPropertyName("metadata")]
        public Metadata Metadata { get; set; }

        [JsonPropertyName("stats")]
        public Stats Stats { get; set; }

        [JsonPropertyName("uploaded")]
        public DateTime Uploaded { get; set; }

        [JsonPropertyName("ranked")]
        public bool Ranked { get; set; }

        [JsonPropertyName("stars")]
        public decimal Stars { get; set; }

        [JsonPropertyName("qualified")]
        public bool Qualified { get; set; }

        [JsonPropertyName("versions")]
        public List<Version> Versions { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("lastPublishedAt")]
        public DateTime LastPublishedAt { get; set; }

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; }
    }
}