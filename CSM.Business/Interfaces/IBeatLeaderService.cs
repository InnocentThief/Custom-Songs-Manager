using CSM.DataAccess.BeatLeader;

namespace CSM.Business.Interfaces
{
    internal interface IBeatLeaderService
    {
        Task<Player?> GetPlayerProfileAsync(string id);

        Task<bool> PlayerExistsAsync(string id);
    }
}
