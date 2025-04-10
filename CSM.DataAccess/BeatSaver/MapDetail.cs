using System.Text.Json.Serialization;

namespace CSM.DataAccess.BeatSaver
{
    internal class MapDetail
    {
        [JsonPropertyName("automapper")]
        public bool Automapper { get; set; }

        [JsonPropertyName("blQualified")]
        public bool BlQualified { get; set; }

        [JsonPropertyName("blRanked")]
        public bool BlRanked { get; set; }

        [JsonPropertyName("bookmarked")]
        public bool Bookmarked { get; set; }

        [JsonPropertyName("collaborators")]
        public List<UserDetail> Collaborators { get; set; } = [];

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("curatedAt")]
        public DateTime CuratedAt { get; set; }

        [JsonPropertyName("curator")]
        public UserDetail? Curator { get; set; }

        [JsonPropertyName("declaredAi")]
        public DeclaredAi DeclaredAi { get; set; } = DeclaredAi.None;

        [JsonPropertyName("deletedAt")]
        public DateTime DeletedAt { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("lastPublishedAt")]
        public DateTime LastPublishedAt { get; set; }

        [JsonPropertyName("metadata")]
        public MapDetailMetadata? Metadata { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("nsfw")]
        public bool Nsfw { get; set; }

        [JsonPropertyName("qualified")]
        public bool Qualified { get; set; }

        [JsonPropertyName("ranked")]
        public bool Ranked { get; set; }

        [JsonPropertyName("stats")]
        public MapStats? Stats { get; set; }

        [JsonPropertyName("tags")]
        public List<Tag> Tags { get; set; } = [];

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("uploaded")]
        public DateTime Uploaded { get; set; }

        [JsonPropertyName("uploader")]
        public UserDetail? Uploader { get; set; }

        [JsonPropertyName("versions")]
        public List<MapVersion> Versions { get; set; } = [];
    }
}
