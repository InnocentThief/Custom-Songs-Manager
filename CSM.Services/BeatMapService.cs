using CSM.DataAccess.Entities.Online;
using System.Collections.Generic;
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
            var beatmap = await client.GetAsync<BeatMap>($"/{key}");
            return beatmap;
        }

        public async Task<BeatMaps> GetBeatMapsByUserIdAsync(int id)
        {
            var beatmaps = await client.GetAsync<BeatMaps>($"/maps/uploader/{id}/0");
            return beatmaps;
        }

        public async Task<User> GetUserByNameAsync(string name)
        {
            var user = await client.GetAsync<User>($"/users/name/{name}");
            return user;
        }
    }
}