using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Online
{
    public class BeatMaps
    {
        [JsonPropertyName("docs")]
        public List<BeatMap> Docs { get; set; }
    }
}