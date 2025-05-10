namespace CSM.DataAccess.UserConfiguration
{
    internal class LeaderboardsConfig
    {
        public string BeatLeaderUserId { get; set; } = string.Empty;

        public LeaderboardType DefaultLeaderboard { get; set; } = LeaderboardType.ScoreSaber;

        public string ScoreSaberUserId { get; set; } = string.Empty;

        public bool UseBeatLeaderLeaderboard { get; set; }

        public bool UseScoreSaberLeaderboard { get; set; }
    }
}
