using System.Threading.Tasks;

namespace CSM.Services
{
    public class BeatMapService
    {
        private readonly GenericServiceClient client;

        public BeatMapService()
        {
            client = new GenericServiceClient("https://api.beatsaver.com/maps/id");
        }

        //public async Task<BeatMap> GetBeatMapDataAsync(string key)
        //{
        //    var beatmap = await client.GetAsync<BeatMap>($"/{key}");
        //    return beatmap;
        //}
    }
}