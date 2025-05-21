namespace CSM.DataAccess.ScoreSaber
{
    internal class PlayerScore
    {
        public Score Score { get; set; } = new Score();
        public LeaderboardInfo Leaderboard { get; set; } = new LeaderboardInfo();
    }
}
