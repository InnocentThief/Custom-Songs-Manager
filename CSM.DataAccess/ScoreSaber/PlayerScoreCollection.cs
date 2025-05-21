namespace CSM.DataAccess.ScoreSaber
{
    internal class PlayerScoreCollection
    {
        public List<PlayerScore> PlayerScores { get; set; } = [];
        public Metadata Metadata { get; set; } = new Metadata();
    }
}
