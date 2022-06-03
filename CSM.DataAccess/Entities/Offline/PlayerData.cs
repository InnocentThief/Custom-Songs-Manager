using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Offline
{
    /// <summary>
    /// Represents the player data as stored in PlayerData.dat.
    /// </summary>
    public class PlayerData
    {
        [JsonPropertyName("localPlayers")]
        public List<LocalPlayer> LocalPlayers { get; set; }
    }
}