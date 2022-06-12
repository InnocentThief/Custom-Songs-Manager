using CSM.Business.TwitchIntegration.TwitchConfiguration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSM.Business.TwitchIntegration
{
    public class TwitchConnectionManager
    {
        private HttpServer httpServer;
        private string redirect_uri = "http://localhost:8080/";
        private string client_id = "mf66rq31qva9bv7dit1jygdjs39loa";
        private readonly List<string> scopes = new List<string> { "chat:read", "chat:edit" };


        public async Task GetAccessTokenAsync()
        {

            if (string.IsNullOrWhiteSpace(TwitchConfigManager.Instance.Config.AccessToken))
            {
                InitializeWebServer();
                var authURL = $"https://id.twitch.tv/oauth2/authorize?response_type=token&client_id={client_id}&redirect_uri={redirect_uri}&scope={string.Join("+", scopes)}";
                System.Diagnostics.Process.Start(authURL);
            }
            else
            {

            }
        }

        public void InitializeWebServer()
        {
            httpServer = new HttpServer();
            httpServer.Start();
        }


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
