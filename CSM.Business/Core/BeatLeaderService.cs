using CSM.Business.Interfaces;
using CSM.DataAccess.BeatLeader;

namespace CSM.Business.Core
{
    internal class BeatLeaderService : IBeatLeaderService
    {
        private readonly GenericServiceClient client = new("https://api.beatleader.com/");

        public async Task<Player?> GetPlayerProfileAsync(string id)
        {
            return await client.GetAsync<Player>($"/player/{id}");
        }

        public async Task<bool> PlayerExistsAsync(string id)
        {
            var response = await client.GetAsync($"/player/{id}/exists");
            return response.IsSuccessStatusCode;
        }
    }
}
