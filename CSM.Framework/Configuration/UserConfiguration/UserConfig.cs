using System.Collections.Generic;

namespace CSM.Framework.Configuration.UserConfiguration
{
    /// <summary>
    /// Represents a user config.
    /// </summary>
    public class UserConfig
    {
        /// <summary>
        /// Gets or sets the Beat Saber install path.
        /// </summary>
        public string BeatSaberInstallPath { get; set; }

        /// <summary>
        /// Gets or sets the Beat Saber custom levels paths.
        /// </summary>
        /// <remarks>Used as list for future expansion to multiple custom level paths.</remarks>
        public List<CustomLevelPath> CustomLevelPaths { get; set; }

        /// <summary>
        /// Gets or sets the Beat Saber playlist paths.
        /// </summary>
        /// <remarks>Used as list for future expansion to multiple playlist paths.</remarks>
        public List<PlaylistPath> PlaylistPaths { get; set; }

        /// <summary>
        /// Gets or sets the default workspace.
        /// </summary>
        public WorkspaceType DefaultWorkspace { get; set; }

        /// <summary>
        /// Gets or sets song detail position on the custom level workspace.
        /// </summary>
        public SongDetailPosition CustomLevelsSongDetailPosition { get; set; }
    }
}