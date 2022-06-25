using CSM.DataAccess.Entities.Online;
using CSM.Framework.Configuration.UserConfiguration;
using System.Threading.Tasks;

namespace CSM.Services
{
    /// <summary>
    /// Client side service for BeatSaver API.
    /// </summary>
    public class BeatMapService
    {
        private readonly GenericServiceClient client;

        /// <summary>
        /// Initialiizes a new <see cref="BeatMapService"/>
        /// </summary>
        /// <param name="api">The base api.</param>
        public BeatMapService(string api)
        {
            client = new GenericServiceClient($"{UserConfigManager.Instance.Config.BeatSaverAPIEndpoint}{api}");
        }

        /// <summary>
        /// Gets the BeatMapData for the given key.
        /// </summary>
        /// <param name="key">BSR key.</param>
        /// <returns>The BeatMap for the given key.</returns>
        public async Task<BeatMap> GetBeatMapDataAsync(string key)
        {
            return await client.GetAsync<BeatMap>($"/{key}");
        }

        /// <summary>
        /// Gets the beatmaps for the given user id.
        /// </summary>
        /// <param name="id">Id of the user.</param>
        /// <returns>A collection of beatmaps for the given user id.</returns>
        public async Task<BeatMaps> GetBeatMapsByUserIdAsync(int id)
        {
            return await client.GetAsync<BeatMaps>($"/maps/uploader/{id}/0");
        }

        /// <summary>
        /// Gets the user for a given user name.
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <returns>The user for the given user name.</returns>
        public async Task<User> GetUserByNameAsync(string name)
        {
            return await client.GetAsync<User>($"/users/name/{name}");
        }

        /// <summary>
        /// Searches for beatmaps on BeatSaver.
        /// </summary>
        /// <param name="query">The query used for the search.</param>
        /// <returns>A holder with a list of beatmaps for the given query.</returns>
        public async Task<BeatMaps> SearchSongsAsync(string query)
        {
            return await client.GetAsync<BeatMaps>($"?{query}");
        }
    }
}