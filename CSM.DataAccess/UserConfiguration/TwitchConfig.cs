namespace CSM.DataAccess.UserConfiguration
{
    internal class TwitchConfig
    {
        public bool RemoveReceivedSongAfterAddingToPlaylist { get; set; }

        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string Login { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
    }
}
