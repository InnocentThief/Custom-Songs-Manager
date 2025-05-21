using CSM.DataAccess.ScoreSaber;

namespace CSM.Business.Interfaces
{
    internal interface IScoreSaberService
    {
        Task<Player?> GetPlayerProfileAsync(string id);
        Task<PlayerCollection?> GetPlayersAsync(string name);
        Task<PlayerScoreCollection?> GetPlayerScoresAsync(string id, int page, int count);
    }
}
