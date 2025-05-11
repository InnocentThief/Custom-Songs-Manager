using CSM.DataAccess.BeatLeader;

namespace CSM.Business.Interfaces
{
    internal interface IBeatLeaderService
    {
        Task<Player?> GetPlayerProfileAsync(string id);

        Task<PlayerSearchResult?> GetPlayersAsync(string name);

        Task<ScoreSearchResult?> GetPlayerScoresAsync(string id, int page, int count);

        Task<bool> PlayerExistsAsync(string id);
    }
}
