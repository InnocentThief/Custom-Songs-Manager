namespace CSM.Business.Core.BeatLeader
{
    internal class BeatLeaderOAuthSettingsTemplate
    {
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string Authority { get; set; } = "https://api.beatleader.com/oauth2/authorize";
        public string RedirectUri { get; set; } = "http://localhost:57790/";
        public string Scopes { get; set; } = "profile";
    }
}
