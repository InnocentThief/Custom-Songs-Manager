using System.Text.Json.Serialization;

namespace CSM.DataAccess.BeatSaber
{
    internal class PlayerData
    {
        [JsonPropertyName("localPlayers")]
        public List<LocalPlayer> LocalPlayers { get; set; } = [];
    }
}
