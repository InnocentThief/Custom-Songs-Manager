namespace CSM.DataAccess.UserConfiguration
{
    internal class PlaylistsConfig
    {
        public bool Available { get; set; } = true;
        public PlaylistPath PlaylistPath { get; set; } = new();
    }
}
