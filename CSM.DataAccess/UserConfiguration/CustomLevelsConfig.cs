using CSM.Framework.Types;

namespace CSM.DataAccess.UserConfiguration
{
    internal class CustomLevelsConfig
    {
        public bool Available { get; set; } = true;
        public CustomLevelPath CustomLevelPath { get; set; } = new();
        public SongDetailPosition SongDetailPosition { get; set; }
    }
}
