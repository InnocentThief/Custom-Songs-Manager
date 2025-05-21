using CSM.Business.Interfaces;
using CSM.DataAccess.ScoreSaber;

namespace CSM.Business.Core
{
    internal class ScoreSaberService : IScoreSaberService
    {
        private readonly GenericServiceClient client = new("https://scoresaber.com/api/");

        public async Task<Player?> GetPlayerProfileAsync(string id)
        {
            return await client.GetAsync<Player>($"player/{id}/full");
        }

        public async Task<PlayerCollection?> GetPlayersAsync(string name)
        {
            return await client.GetAsync<PlayerCollection>($"players?search={name}");
        }

        public async Task<PlayerScoreCollection?> GetPlayerScoresAsync(string id, int page, int count)
        {
            return await client.GetAsync<PlayerScoreCollection>($"player/{id}/scores?sort=top&page={page}&limit={count}");
        }
    }
}
