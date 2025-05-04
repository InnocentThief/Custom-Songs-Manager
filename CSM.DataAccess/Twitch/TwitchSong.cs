namespace CSM.DataAccess.Twitch
{
    internal class TwitchSong
    {
        public string ChannelName { get; set; } = string.Empty;
        public DateTime? ReceivedAt { get; set; }
        public string Key { get; set; } = string.Empty;
    }
}
