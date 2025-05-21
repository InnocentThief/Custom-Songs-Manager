using CSM.Business.Interfaces;
using CSM.DataAccess.BeatLeader;

namespace CSM.Business.Core
{
    internal class BeatLeaderService : IBeatLeaderService
    {
        private readonly GenericServiceClient client = new("https://api.beatleader.com/");

        public async Task<Player?> GetPlayerProfileAsync(string id)
        {
            return await client.GetAsync<Player>($"player/{id}");
        }

        public async Task<PlayerSearchResult?> GetPlayersAsync(string name)
        {
            return await client.GetAsync<PlayerSearchResult>($"players?search={name}");
        }

        public async Task<ScoreSearchResult?> GetPlayerScoresAsync(string id, int page, int count)
        {
            return await client.GetAsync<ScoreSearchResult>($"player/{id}/scores?page={page}&count={count}");
        }

        public async Task<bool> PlayerExistsAsync(string id)
        {
            var response = await client.GetAsync($"player/{id}/exists");
            return response.IsSuccessStatusCode;
        }
    }
}
