namespace CSM.DataAccess.UserConfiguration
{
    public record PlaylistPath
    {
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public bool Default { get; set; }
    }
}
