using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.DataAccess.UserConfiguration
{
    internal class TwitchConfig
    {
        public bool Available { get; set; } = true;
        public bool RemoveReceivedSongAfterAddingToPlaylist { get; set; }
    }
}
