using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CSM.Services
{
    /// <summary>
    /// Provides client-side REST service handling.
    /// </summary>
    internal sealed class GenericServiceClient
    {
        #region Private fields

        private readonly string apiBaseAddress;
        private static readonly HttpClient client = new HttpClient();

        #endregion

        /// <summary>
        /// Creates a new <see cref="GenericServiceClient"/>.
        /// </summary>
        /// <param name="apiBaseAddress">The base api address for the client.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="apiBaseAddress"/> is null.</exception>
        internal GenericServiceClient(string apiBaseAddress)
        {
            this.apiBaseAddress = apiBaseAddress ?? throw new ArgumentNullException(nameof(apiBaseAddress));
        }

        /// <summary>
        /// Gets the resource of the address provided.
        /// </summary>
        /// <typeparam name="T">Type of the resource to get.</typeparam>
        /// <param name="api">Address of the resource.</param>
        /// <returns>Resource for the address provided.</returns>
        internal async Task<T> GetAsync<T>(string api)
        {
            Debug.WriteLine($"GenericServiceClient.GetAsync({apiBaseAddress + api})");
            var response = await client.GetAsync(apiBaseAddress + api);
            if (!response.IsSuccessStatusCode)
            {
                return default;
            }
            var value = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(value);
        }
    }
}