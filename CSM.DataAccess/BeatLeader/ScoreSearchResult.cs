namespace CSM.DataAccess.BeatLeader
{
    internal class ScoreSearchResult
    {
        public Metadata Metadata { get; set; } = new();
        public List<Score> Data { get; set; } = [];
    }
}
