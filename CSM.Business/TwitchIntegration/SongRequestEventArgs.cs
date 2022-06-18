namespace CSM.Business.TwitchIntegration
{
    /// <summary>
    /// EventArgs used for song requests.
    /// </summary>
    public class SongRequestEventArgs
    {
        /// <summary>
        /// Gets or sets the name of the channel the song was requested in.
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// Gets or sets the key of the requested song.
        /// </summary>
        public string Key { get; set; }
    }
}