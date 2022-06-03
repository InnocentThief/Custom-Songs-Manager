namespace CSM.Framework.Configuration.UserConfiguration
{
    /// <summary>
    /// Represents one playlist path inside the settings.
    /// </summary>
    public class PlaylistPath
    {
        /// <summary>
        /// Gets or sets the name of the path setting.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets whether the path is the default one.
        /// </summary>
        public bool Default { get; set; }
    }
}