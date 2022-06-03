namespace CSM.Framework.Configuration.UserConfiguration
{
    /// <summary>
    /// EventArgs used for user config changes.
    /// </summary>
    public class UserConfigChangedEventArgs
    {
        /// <summary>
        /// Gets or sets whether the custom levels path has changed.
        /// </summary>
        public bool CustomLevelsPathChanged { get; set; }

        /// <summary>
        /// Gets or sets whether the custom level detail position has changed.
        /// </summary>
        public bool CustomLevelDetailPositionChanged { get; set; }

        /// <summary>
        /// Gets or sets whether the playlists path has changed.
        /// </summary>
        public bool PlaylistsPathChanged { get; set; }
    }
}