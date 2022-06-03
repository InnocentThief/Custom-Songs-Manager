using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Online
{
    /// <summary>
    /// Represents the return value of a search request. Containing a list of BeatMaps.
    /// </summary>
    public class BeatMaps
    {
        [JsonPropertyName("docs")]
        public List<BeatMap> Docs { get; set; }
    }
}