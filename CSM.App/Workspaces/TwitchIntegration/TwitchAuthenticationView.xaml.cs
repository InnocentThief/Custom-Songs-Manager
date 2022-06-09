using CSM.Business.TwitchIntegration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CSM.App.Workspaces.TwitchIntegration
{
    /// <summary>
    /// Interaction logic for TwitchAuthenticationView.xaml
    /// </summary>
    public partial class TwitchAuthenticationView : Window
    {
        private string redirect_uri = "http://localhost:7681/";
        private string client_id = "mf66rq31qva9bv7dit1jygdjs39loa";
        private readonly List<string> scopes = new List<string> { "chat:read", "chat:edit" };


        public TwitchAuthenticationView()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            TwitchConnectionManager.Instance.InitializeWebServer();


            var authURL = $"https://id.twitch.tv/oauth2/authorize?response_type=token&client_id={client_id}&redirect_uri={redirect_uri}&scope={string.Join("+", scopes)}";
            webBrowser.Source = new Uri(authURL);
        }

        public string GetUrl()
        {
            return webBrowser.Source.ToString();
        }
    }
}
