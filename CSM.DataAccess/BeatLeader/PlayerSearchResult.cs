namespace CSM.DataAccess.BeatLeader
{
    internal class PlayerSearchResult
    {
        public Metadata Metadata { get; set; } = new();
        public List<Player> Data { get; set; } = [];
    }
}