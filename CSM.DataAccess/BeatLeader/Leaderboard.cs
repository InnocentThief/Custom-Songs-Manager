namespace CSM.DataAccess.BeatLeader
{
    internal class Leaderboard
    {
        public Song Song { get; set; } = new();
        public Difficulty Difficulty { get; set; } = new();
    }
}
