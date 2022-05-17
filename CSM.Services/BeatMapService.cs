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
            var beatmap = await client.GetAsync<BeatMap>($"/{key}");
            return beatmap;
        }
    }
}