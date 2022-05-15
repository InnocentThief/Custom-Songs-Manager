using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.Framework.Configuration.UserConfiguration
{
    public class UserConfig
    {
        public string BeatSaberInstallPath { get; set; }

        public List<CustomLevelPath> CustomLevelPaths { get; set; }

        public List<PlaylistPath> PlaylistPaths { get; set; }

        public WorkspaceType DefaultWorkspace { get; set; }

        public SongDetailPosition CustomLevelsSongDetailPosition { get; set; }
    }
}