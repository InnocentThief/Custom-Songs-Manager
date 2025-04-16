namespace CSM.DataAccess.UserConfiguration
{
    internal class SongSuggestConfig
    {
        public string ScoreSaberUserId { get; set; } = string.Empty;
        public string BeatLeaderUserId { get; set; } = string.Empty;
        public bool UseScoreSaberLeaderboard { get; set; }
        public bool UseBeatLeaderLeaderboard { get; set; }
        public LeaderboardType DefaultLeaderboard { get; set; } = LeaderboardType.ScoreSaber;
        public SongSuggestSettings SongSuggestSettings { get; set; } = new SongSuggestSettings();
    }
}
