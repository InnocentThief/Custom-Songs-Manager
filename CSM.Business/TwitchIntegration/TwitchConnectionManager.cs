using CSM.Business.TwitchIntegration.TwitchConfiguration;
using CSM.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSM.Business.TwitchIntegration
{
    public class TwitchConnectionManager
    {
        //private HttpServer httpServer;
        private HttpListener listener;
        private string redirect_uri = "http://localhost:8080/";
        private string client_id = "mf66rq31qva9bv7dit1jygdjs39loa";
        private readonly List<string> scopes = new List<string> { "chat:read", "chat:edit" };
        private TwitchService twitchService;

        private TwitchConnectionManager()
        {
            twitchService = new TwitchService();
        }

        public async Task<bool> ValidateAsync()
        {
            var validationResponse = await twitchService.ValidateAsync(TwitchConfigManager.Instance.Config.AccessToken);
            if (validationResponse != null)
            {
                TwitchConfigManager.Instance.Config.Login = validationResponse.Login;
                TwitchConfigManager.Instance.Config.UserId = validationResponse.UserId;
                return true;
            }
            else
            {
                TwitchConfigManager.Instance.Config.AccessToken = string.Empty;
                TwitchConfigManager.Instance.Config.Login = string.Empty;
                TwitchConfigManager.Instance.Config.UserId = string.Empty;
                return false;
            }
        }

        public async Task GetAccessTokenAsync()
        {

            if (string.IsNullOrWhiteSpace(TwitchConfigManager.Instance.Config.AccessToken))
            {
                StartListener();
                var authURL = $"https://id.twitch.tv/oauth2/authorize?response_type=token&client_id={client_id}&redirect_uri={redirect_uri}&scope={string.Join("+", scopes)}";
                System.Diagnostics.Process.Start(authURL);
            }
            await Task.CompletedTask;
        }

        #region Helper methods

        private void StartListener()
        {
            listener = new HttpListener();
            listener.Prefixes.Add(redirect_uri);
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
            if (listener.IsListening)
            {
                var context = listener.EndGetContext(result);
                var request = context.Request;

                if (request.HttpMethod == "GET")
                {
                    CreateResponse(context, "CSM.Business.TwitchIntegration.TwitchGetAccessToken.html");
                }
                else if (request.HttpMethod == "POST")
                {
                    using (StreamReader streamReader = new StreamReader(context.Request.InputStream))
                    {
                        var content = streamReader.ReadToEnd();
                        if (content.Contains("error"))
                        {
                            CreateResponse(context, "CSM.Business.TwitchIntegration.TwitchFailed.html");
                            TwitchConfigManager.Instance.Config.AccessToken = string.Empty;
                        }
                        else
                        {
                            var accessTokenSubstring = content.Substring(content.IndexOf("#access_token="));
                            var accessTokenWithoutHeader = accessTokenSubstring.Replace("#access_token=", "");
                            var accessToken = accessTokenWithoutHeader.Substring(0, accessTokenWithoutHeader.IndexOf("&"));
                            TwitchConfigManager.Instance.Config.AccessToken = accessToken;
                            TwitchConfigManager.Instance.SaveTwitchConfig();
                            CreateResponse(context, "CSM.Business.TwitchIntegration.TwitchSucceeded.html");

                        }
                        //StopListener();
                        //return;
                    }
                }

                Receive();
            }
        }

        private void CreateResponse(HttpListenerContext context, string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    var responseString = reader.ReadToEnd();
                    byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                    var response = context.Response;
                    response.ContentType = "text/html";
                    response.ContentLength64 = buffer.Length;
                    response.StatusCode = 200;
                    response.OutputStream.Write(buffer, 0, buffer.Length);
                    response.OutputStream.Close();
                }
            }
        }

        #endregion

        #region Singleton

        private static TwitchConnectionManager instance;

        public static TwitchConnectionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TwitchConnectionManager();
                }
                return instance;
            }
        }

        #endregion
    }
}
