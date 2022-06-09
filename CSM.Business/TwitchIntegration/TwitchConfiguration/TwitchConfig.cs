using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.Business.TwitchIntegration.TwitchConfiguration
{
    public class TwitchConfig
    {
        public string UserName { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public List<TwitchChannel> Channels { get; set; }
    }
}