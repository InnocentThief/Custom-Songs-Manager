using System.Text.Json.Serialization;

namespace CSM.DataAccess.Twitch
{
    internal class TwitchValidationResponse
    {
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; } = string.Empty;

        [JsonPropertyName("login")]
        public string Login { get; set; } = string.Empty;

        [JsonPropertyName("scopes")]
        public string[] Scopes { get; set; } = [];

        [JsonPropertyName("user_id")]
        public string UserId { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
