using CSM.Framework.Types;

namespace CSM.DataAccess.UserConfiguration
{
    public class UserConfig
    {
        public string BeatSaberInstallPath { get; set; } = string.Empty;
        public string BeatSaverAPIEndpoint { get; set; } = string.Empty;
        public List<CustomLevelPath> CustomLevelPaths { get; set; } = [];
        public bool RemoveReceivedSongAfterAddingToPlaylist { get; set; }
        public List<PlaylistPath> PlaylistPaths { get; set; } = [];
        public NavigationType DefaultWorkspace { get; set; }
        public SongDetailPosition CustomLevelsSongDetailPosition { get; set; }
    }
}
