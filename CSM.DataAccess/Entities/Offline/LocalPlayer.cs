using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Offline
{
    /// <summary>
    /// Represents a local player as stored in PlayerData.dat
    /// </summary>
    public class LocalPlayer
    {
        [JsonPropertyName("favoritesLevelIds")]
        public List<string> FavoritesLevelIds { get; set; }
    }
}