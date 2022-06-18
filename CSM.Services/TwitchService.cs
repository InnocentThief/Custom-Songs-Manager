using CSM.DataAccess.Entities.Online;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace CSM.Services
{
    public class TwitchService
    {
        private readonly HttpClient client;

        public TwitchService()
        {
            client = new HttpClient();

        }

        public async Task<TwitchValidationResponse> ValidateAsync(string token)
        {
            var request = new HttpRequestMessage();
            request.Method = HttpMethod.Get;
            request.Headers.Add("Authorization", $"OAuth {token}");
            request.RequestUri = new Uri("https://id.twitch.tv/oauth2/validate");
            request.Headers.UserAgent.Add(new ProductInfoHeaderValue("Custom-Songs-Manager", Assembly.GetExecutingAssembly().GetName().Version.ToString()));
            var response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var value = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TwitchValidationResponse>(value);
            }
            return null;
        }
    }
}