using System.Diagnostics;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;

namespace CSM.Services
{
    internal sealed class GenericServiceClient
    {
        #region Private fields

        private readonly string apiBaseAddress;
        private static readonly HttpClient httpClient = new();

        #endregion

        public GenericServiceClient(string apiBaseAddress)
        {
            this.apiBaseAddress = apiBaseAddress;
            if (httpClient.DefaultRequestHeaders.UserAgent.Count == 0)
            {
                httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Custom-Songs-Manager", Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString() ?? string.Empty));
            }
        }

        public async Task<T?> GetAsync<T>(string api)
        {
            Debug.WriteLine($"GET {apiBaseAddress + api}");
            var response = await httpClient.GetAsync(apiBaseAddress + api);
            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"Error: {response.StatusCode}");
                return default;
            }
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content);
        }
    }
}
