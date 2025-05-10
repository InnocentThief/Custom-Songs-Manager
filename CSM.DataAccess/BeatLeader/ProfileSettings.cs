namespace CSM.DataAccess.BeatLeader
{
    internal class ProfileSettings
    {
        public int Id { get; set; }
        public string? Bio { get; set; }
        public string? Message { get; set; }
        public string EffectName { get; set; } = string.Empty;
        public string ProfileAppearance { get; set; } = string.Empty;
        public object? Hue { get; set; }
        public object? Saturation { get; set; }
        public object? LeftSaberColor { get; set; }
        public object? RightSaberColor { get; set; }
        public object? ProfileCover { get; set; }
        public string StarredFriends { get; set; } = string.Empty;
        public bool HorizontalRichBio { get; set; }
        public object? RankedMapperSort { get; set; }
        public bool ShowBots { get; set; }
        public bool ShowAllRatings { get; set; }
        public bool ShowExplicitCovers { get; set; }
        public bool ShowStatsPublic { get; set; }
        public bool ShowStatsPublicPinned { get; set; }
    }
}
