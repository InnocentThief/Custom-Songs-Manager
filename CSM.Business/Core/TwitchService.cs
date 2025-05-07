using CSM.Business.Interfaces;
using CSM.DataAccess.Twitch;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace CSM.Business.Core
{
    internal class TwitchService(IUserConfigDomain userConfigDomain) : ITwitchService
    {
        private readonly HttpClient client = new();
        private readonly HttpListener listener = new();
        private readonly string redirect_uri = "http://localhost:57789/";
        private readonly string client_id = "";
        private readonly List<string> scopes = ["chat:read", "chat:edit"];

        public async Task GetAccessTokenAsync()
        {
            if (string.IsNullOrWhiteSpace(userConfigDomain.Config?.TwitchConfig.AccessToken))
            {
                StartListener();
                var authUrl = $"https://id.twitch.tv/oauth2/authorize?response_type=token&client_id={client_id}&redirect_uri={redirect_uri}&scope={string.Join("+", scopes)}";
                Process.Start(new ProcessStartInfo(authUrl) { UseShellExecute = true });
            }
            await Task.CompletedTask;
        }

        public async Task<bool> ValidateAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://id.twitch.tv/oauth2/validate");
            request.Headers.Add("Authorization", $"OAuth {userConfigDomain.Config?.TwitchConfig.AccessToken ?? string.Empty}");
            request.Headers.UserAgent.Add(new ProductInfoHeaderValue("Custom-Songs-Manager", Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString()));
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var value = await response.Content.ReadAsStringAsync();
                var twitchValidationResponse = JsonSerializer.Deserialize<TwitchValidationResponse>(value);
                if (twitchValidationResponse == null)
                    return false;

                userConfigDomain.Config!.TwitchConfig.Login = twitchValidationResponse.Login;
                userConfigDomain.Config.TwitchConfig.UserId = twitchValidationResponse.UserId;
                userConfigDomain.Config!.TwitchConfig.Username = twitchValidationResponse.Login;
                userConfigDomain.SaveUserConfig();
                return true;
            }

            userConfigDomain.Config!.TwitchConfig.AccessToken = string.Empty;
            userConfigDomain.Config.TwitchConfig.RefreshToken = string.Empty;
            userConfigDomain.Config.TwitchConfig.Login = string.Empty;
            userConfigDomain.Config.TwitchConfig.UserId = string.Empty;
            userConfigDomain.SaveUserConfig();
            return false;
        }

        #region Helper methods

        private void StartListener()
        {
            listener.Prefixes.Add("http://localhost:57789/");
            listener.Start();
            Receive();
        }

        private void StopListener()
        {
            listener.Stop();
        }

        private void Receive()
        {
            listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
        }

        private void ListenerCallback(IAsyncResult result)
        {
            if (!listener.IsListening)
                return;

            var context = listener.EndGetContext(result);
            var request = context.Request;

            if (request.HttpMethod == "GET")
            {
                CreateResponse(context, "CSM.Business.Core.Twitch.TwitchGetAccessToken.html");
            }
            else if (request.HttpMethod == "POST")
            {
                using StreamReader streamReader = new(context.Request.InputStream);
                var content = streamReader.ReadToEnd();
                if (content.Contains("error"))
                {
                    userConfigDomain.Config!.TwitchConfig.AccessToken = string.Empty;
                }
                else
                {
                    var accessTokenSubstring = content[content.IndexOf("#access_token=")..];
                    var accessTokenWithoutHeader = accessTokenSubstring.Replace("#access_token=", "");
                    var accessToken = accessTokenWithoutHeader[..accessTokenWithoutHeader.IndexOf('&')];
                    userConfigDomain.Config!.TwitchConfig.AccessToken = accessToken;
                    userConfigDomain.SaveUserConfig();
                }
                StopListener();
                return;
            }

            Receive();
        }

        private static void CreateResponse(HttpListenerContext context, string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using Stream? stream = assembly.GetManifestResourceStream(resourceName);
            if (stream is null)
                return;
            using StreamReader reader = new(stream);
            var responseString = reader.ReadToEnd();
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            var response = context.Response;
            response.ContentType = "text/html";
            response.ContentLength64 = buffer.Length;
            response.StatusCode = 200;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }

        #endregion
    }
}
