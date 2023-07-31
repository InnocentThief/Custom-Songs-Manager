using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Offline
{
    /// <summary>
    /// Represents the custom data including hitbloq url.
    /// </summary>
    public class CustomData
    {
        [JsonPropertyName("syncURL")]
        public string SyncURL { get; set; }

        [JsonPropertyName("AllowDuplicates")]
        public bool AllowDuplicates { get; set; }
    }
}