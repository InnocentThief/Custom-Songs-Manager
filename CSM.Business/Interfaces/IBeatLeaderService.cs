using CSM.DataAccess.BeatLeader;

namespace CSM.Business.Interfaces
{
    internal interface IBeatLeaderService
    {
        Task<Player?> GetPlayerProfileAsync(string id);

        Task<List<string>> GetPlayerScoresAsync(string id);

        Task<bool> PlayerExistsAsync(string id);
    }
}
