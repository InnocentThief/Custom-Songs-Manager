using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.Framework.Configuration.UserConfiguration
{
    public class UserConfigChangedEventArgs
    {
        public bool CustomLevelsPathChanged { get; set; }

        public bool CustomLevelDetailPositionChanged { get; set; }

        public bool PlaylistsPathChanged { get; set; }
    }
}
