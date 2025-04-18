using CSM.Business.Interfaces;
using CSM.DataAccess.BeatSaver;

namespace CSM.Business.Core
{
    internal class BeatSaverService(IUserConfigDomain userConfigDomain) : IBeatSaverService
    {
        private readonly GenericServiceClient client = new(userConfigDomain.Config?.BeatSaverAPIEndpoint ?? "https://api.beatsaver.com/");

        public async Task<MapDetail?> GetMapDetailAsync(string id)
        {
            return await client.GetAsync<MapDetail>($"/maps/id/{id}");
        }

        public async Task<Dictionary<string, MapDetail>?> GetMapDetailsAsync(List<string> keys, BeatSaverKeyType keyType)
        {
            return keyType switch
            {
                BeatSaverKeyType.Id => await client.GetAsync<Dictionary<string, MapDetail>>($"/maps/ids/{string.Join(",", keys)}"),
                BeatSaverKeyType.Hash => await client.GetAsync<Dictionary<string, MapDetail>>($"/maps/hash/{string.Join(",", keys)}"),
                _ => throw new ArgumentOutOfRangeException(nameof(keyType), keyType, null),
            };
        }

        public async Task<MapDetails?> SearchAsync(string query)
        {
            return await client.GetAsync<MapDetails>($"/search/text/{query}");
        }
    }
}
