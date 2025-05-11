namespace CSM.UiLogic.ViewModels.Common.Leaderboard
{
    internal class SearchResultEventArgs(string? playerId) : EventArgs
    {
        public string? PlayerId { get; set; } = playerId;
    }
}
