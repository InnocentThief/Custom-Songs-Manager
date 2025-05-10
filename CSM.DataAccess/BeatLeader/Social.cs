namespace CSM.DataAccess.BeatLeader
{
    internal class Social
    {
        public int Id { get; set; }
        public string Service { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string PlayerId { get; set; } = string.Empty;
        public bool Hidden { get; set; }
    }
}
