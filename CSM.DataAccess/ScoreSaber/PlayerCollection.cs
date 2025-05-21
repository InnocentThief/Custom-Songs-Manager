namespace CSM.DataAccess.ScoreSaber
{
    internal class PlayerCollection
    {
        public Metadata Metadata { get; set; } = new Metadata();
        public List<Player> Players { get; set; } = [];
    }
}
