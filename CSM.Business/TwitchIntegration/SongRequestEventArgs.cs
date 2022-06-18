using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.Business.TwitchIntegration
{
    public class SongRequestEventArgs
    {
        public string ChannelName { get; set; }

        public string Key { get; set; }
    }
}