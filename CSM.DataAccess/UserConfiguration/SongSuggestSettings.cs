namespace CSM.DataAccess.UserConfiguration
{
    internal class SongSuggestSettings
    {
        public string SuggestionName = "Custom Songs Manager";
        public bool IgnorePlayedAll { get; set; } = false;
        public int IgnorePlayedDays { get; set; } = 14;
        public bool IgnoreNonImprovable { get; set; } = true;
        public bool UseLikedSongs { get; set; } = false;
        public bool FillLikedSongs { get; set; } = true;
        public bool UseLocalScores { get; set; } = false;
        public int ExtraSongs { get; set; } = 15;
        public int PlaylistLength { get; set; } = 50;
        public LeaderboardType LeaderboardType { get; set; } = LeaderboardType.ScoreSaber;
        public int OriginSongCount { get; set; } = 50;
        public SongSuggestFilterSettings FilterSettings { get; } = new SongSuggestFilterSettings();
        public SongSuggestPlaylistSettings PlaylistSettings { get; } = new SongSuggestPlaylistSettings();
    }
}
