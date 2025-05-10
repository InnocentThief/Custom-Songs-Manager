namespace CSM.DataAccess.BeatLeader
{
    internal class Player
    {
        public int MapperId { get; set; }
        public bool Banned { get; set; }
        public bool Inactive { get; set; }
        public string? BanDescription { get; set; }
        public string ExternalProfileUrl { get; set; } = string.Empty;
        public int RichBioTimeset { get; set; }
        public int SpeedrunStart { get; set; }
        public object? LinkedIds { get; set; }
        public object? History { get; set; }
        public List<object> Badges { get; set; } = new();
        public object? PinnedScores { get; set; }
        public List<object> Changes { get; set; } = new();
        public double AccPp { get; set; }
        public double PassPp { get; set; }
        public double TechPp { get; set; }
        public double AllContextsPp { get; set; }
        public ScoreStats ScoreStats { get; set; } = new();
        public double LastWeekPp { get; set; }
        public int LastWeekRank { get; set; }
        public int LastWeekCountryRank { get; set; }
        public int ExtensionId { get; set; }
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Platform { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string? Alias { get; set; }
        public bool Bot { get; set; }
        public double Pp { get; set; }
        public int Rank { get; set; }
        public int CountryRank { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public int Prestige { get; set; }
        public string Role { get; set; } = string.Empty;
        public List<Social> Socials { get; set; } = new();
        public object? ContextExtensions { get; set; }
        public object? PatreonFeatures { get; set; }
        public ProfileSettings ProfileSettings { get; set; } = new();
        public string ClanOrder { get; set; } = string.Empty;
        public List<Clan> Clans { get; set; } = new();
    }
}
