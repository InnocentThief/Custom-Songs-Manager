using CSM.DataAccess.Common;
using System.Text.Json.Serialization;

namespace CSM.DataAccess.ScoreSaber
{
    internal class SSDifficulty
    {
        public int LeaderboardId { get; set; }
        public int Difficulty { get; set; }
        public string GameMode { get; set; } = string.Empty;
        [JsonIgnore]
        public Characteristic Characteristic
        {
            get
            {
                return GameMode switch
                {
                    "SoloStandard" => Characteristic.Standard,
                    "SoloOneSaber" => Characteristic.OneSaber,
                    "SoloNoArrows" => Characteristic.NoArrows,
                    "Solo90Degree" => Characteristic.Degree90,
                    "Solo360Degree" => Characteristic.Degree360,
                    "SoloLightshow" => Characteristic.Lightshow,
                    "SoloLawless" => Characteristic.Lawless,
                    _ => Characteristic.Standard
                };
            }
        }

        public string DifficultyRaw { get; set; } = string.Empty;
    }
}
