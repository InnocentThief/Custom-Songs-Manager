using CSM.DataAccess.Entities.Online.ScoreSaber;
using System.Threading.Tasks;

namespace CSM.Services
{
    /// <summary>
    /// Client side service for ScoreSaber API.
    /// </summary>
    public class ScoreSaberService
    {
        private readonly GenericServiceClient client;

        /// <summary>
        /// Initializes a new <see cref="ScoreSaberService"/>.
        /// </summary>
        public ScoreSaberService()
        {
            client = new GenericServiceClient("https://scoresaber.com/api");
        }

        /// <summary>
        /// Gets the full player info for the given player id.
        /// </summary>
        /// <param name="playerId">Id of the player.</param>
        /// <returns>An awaitable task that returns a <see cref="Player"/>.</returns>
        public async Task<Player> GetFullPlayerInfoAsync(string playerId)
        {
            return await client.GetAsync<Player>($"/player/{playerId}/full");
        }

        /// <summary>
        /// Gets a list of players for the given query.
        /// </summary>
        /// <param name="query">Query to use to search for players.</param>
        /// <returns>An awaitable task that return a <see cref="PlayerCollection"/>.</returns>
        public async Task<PlayerCollection> GetPlayersAsync(string query)
        {
            return await client.GetAsync<PlayerCollection>($"/players?{query}");
        }

        
    }
}