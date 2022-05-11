using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Online
{
    public class Version
    {
        [JsonPropertyName("hash")]
        public string Hash { get; set; }

        [JsonPropertyName("diffs")]
        public List<Difficulty> Difficulties { get; set; }
    }
}