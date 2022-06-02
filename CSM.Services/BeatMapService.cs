using CSM.DataAccess.Entities.Online;
using System.Threading.Tasks;

namespace CSM.Services
{
    public class BeatMapService
    {
        private readonly GenericServiceClient client;

        public BeatMapService(string api)
        {
            client = new GenericServiceClient($"https://api.beatsaver.com/{api}");
        }

        public async Task<BeatMap> GetBeatMapDataAsync(string key)
        {
            return await client.GetAsync<BeatMap>($"/{key}");
        }

        public async Task<BeatMaps> GetBeatMapsByUserIdAsync(int id)
        {
            return await client.GetAsync<BeatMaps>($"/maps/uploader/{id}/0");
        }

        public async Task<User> GetUserByNameAsync(string name)
        {
            return await client.GetAsync<User>($"/users/name/{name}");
        }

        public async Task<BeatMaps> SearchSongsAsync(string query)
        {
            return await client.GetAsync<BeatMaps>($"?{query}");
        }
    }
}