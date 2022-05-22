using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Offline
{
    public class PlayerData
    {
        [JsonPropertyName("localPlayers")]
        public List<LocalPlayer> LocalPlayers { get; set; }
    }
}