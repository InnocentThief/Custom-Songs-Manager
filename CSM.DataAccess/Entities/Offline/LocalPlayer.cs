using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CSM.DataAccess.Entities.Offline
{
    public class LocalPlayer
    {
        [JsonPropertyName("favoritesLevelIds")]
        public List<string> FavoritesLevelIds { get; set; }
    }
}