namespace CSM.DataAccess.ScoreSaber
{
    internal class LeaderboardInfo
    {
        public int Id { get; set; }
        public string SongHash { get; set; } = string.Empty;
        public string SongName { get; set; } = string.Empty;
        public string SongSubName { get; set; } = string.Empty;
        public string SongAuthorName { get; set; } = string.Empty;
        public string LevelAuthorName { get; set; } = string.Empty;
        public SSDifficulty Difficulty { get; set; } = new SSDifficulty();
        public int MaxScore { get; set; }
        public string CoverImage { get; set; } = string.Empty;
    }
}
