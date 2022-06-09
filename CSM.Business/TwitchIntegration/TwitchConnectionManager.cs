using CSM.Business.TwitchIntegration.TwitchConfiguration;
using Newtonsoft.Json.Linq;
//using NHttp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Http;

namespace CSM.Business.TwitchIntegration
{
    public class TwitchConnectionManager
    {
        private HttpServer httpServer;
        private string redirect_uri = "http://localhost:7681/";
        private string client_id = "mf66rq31qva9bv7dit1jygdjs39loa";
        private readonly List<string> scopes = new List<string> { "chat:read", "chat:edit" };


        public async Task GetAccessTokenAsync()
        {
            if (string.IsNullOrWhiteSpace(TwitchConfigManager.Instance.Config.AccessToken))
            {
                //InitializeWebServer();

                //httpServer = new HttpServer();
                //httpServer.Start();


                //var authURL = $"https://id.twitch.tv/oauth2/authorize?response_type=token&client_id={client_id}&redirect_uri={redirect_uri}&scope={string.Join("+", scopes)}";
                ////var authURL = $"https://id.twitch.tv/oauth2/authorize?client_id={client_id}&redirect_uri={redirect_uri}&response_type=token&scope=channel%3Amanage%3Apolls+channel%3Aread%3Apolls";
                //System.Diagnostics.Process.Start(authURL);
            }
            else
            {

            }
        }

        public void InitializeWebServer()
        {
            //var httpConfiguration = new HttpConfiguration();

            httpServer = new HttpServer();
            httpServer.Start();


            //httpServer.EndPoint = new IPEndPoint(IPAddress.Loopback, 3000);
            //httpServer.RequestReceived += HttpServer_RequestReceived;
            //httpServer.Start();
        }

        //private async void HttpServer_RequestReceived(object sender, HttpRequestEventArgs e)
        //{
        //    using (var streamWriter = new StreamWriter(e.Response.OutputStream))
        //    {
        //        if (e.Request.QueryString.AllKeys.Any("error".Contains))
        //        {

        //            //await GetAccessAndRefreshTokens(code);
        //        }
        //        else
        //        {
        //            var code = e.Request.QueryString["code"];
        //           //await GetAccessAndRefreshTokens(code);

        //            //TwitchConfigManager.Instance.Config.AccessToken = e.Request.QueryString["access_token"];
        //        }
        //    }
        //}

        //private async Task GetAccessAndRefreshTokens(string code)
        //{
        //    var httpClient = new HttpClient();
        //    var values = new Dictionary<string, string>
        //    {
        //        {"client_id", "mf66rq31qva9bv7dit1jygdjs39loa" },
        //        {"code", code },
        //        {"grant_type", "authorization_code" },
        //        {"redirect_url", "http://localhost" }
        //    };
        //    var content = new FormUrlEncodedContent(values);
        //    var response = await httpClient.PostAsync("https://id.twitch.tv/oauth2/token", content);
        //    var responseString = await response.Content.ReadAsStringAsync();
        //    var json = JObject.Parse(responseString);
        //    TwitchConfigManager.Instance.Config.AccessToken = json["access_token"].ToString();
        //    TwitchConfigManager.Instance.Config.RefreshToken = json["refresh_token"].ToString();

        //    httpServer.Stop();
        //    httpServer.Dispose();
        //}

        #region Singleton

        private static TwitchConnectionManager instance;

        public static TwitchConnectionManager Instance
        {
            get
            {
                if (instance == null) instance = new TwitchConnectionManager();
                return instance;
            }
        }

        #endregion
    }
}
