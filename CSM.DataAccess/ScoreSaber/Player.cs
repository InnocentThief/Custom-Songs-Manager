namespace CSM.DataAccess.ScoreSaber
{
    internal class Player
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public double PP { get; set; }
        public int Rank { get; set; }
        public int CountryRank { get; set; }
        public string Role { get; set; } = string.Empty;
        public List<Badge> Badges { get; set; } = [];
        public string Histories { get; set; } = string.Empty;
        public ScoreStats ScoreStats { get; set; } = new ScoreStats();
        public double Permissions { get; set; }
        public bool Banned { get; set; }
        public bool Inactive { get; set; }
        public DateTime FirstSeen { get; set; }

    }
}
