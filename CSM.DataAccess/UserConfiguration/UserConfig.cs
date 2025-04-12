using CSM.Framework.Types;

namespace CSM.DataAccess.UserConfiguration
{
    internal class UserConfig
    {
        public string BeatSaberInstallPath { get; set; } = string.Empty;
        public string BeatSaverAPIEndpoint { get; set; } = string.Empty;
        public NavigationType DefaultWorkspace { get; set; }
        public CustomLevelsConfig CustomLevelsConfig { get; set; } = new();
        public PlaylistsConfig PlaylistsConfig { get; set; } = new();
        public TwitchConfig TwitchConfig { get; set; } = new();
        public ScoreSaberConfig ScoreSaberConfig { get; set; } = new();
        public BeatLeaderConfig BeatLeaderConfig { get; set; } = new();
    }
}
