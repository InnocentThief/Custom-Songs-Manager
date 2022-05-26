using CSM.DataAccess.Entities.Offline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.Tools.CleanupCustomLevels
{
    public class CustomLevelViewModel
    {
        private CustomLevel customLevel;

        public string SongName => customLevel.SongName;

        public string LevelAuthorName => customLevel.LevelAuthorName;

        public string SongAuthorName => customLevel.SongAuthorName;

        public DateTime ChangeDate => customLevel.ChangeDate;

        public string ErrorFound
        {
            get
            {
                if (string.IsNullOrWhiteSpace(customLevel.BsrKey)) return "Missing BSR Key";
                try
                {
                    var hex = int.Parse(customLevel.BsrKey, System.Globalization.NumberStyles.HexNumber);
                }
                catch (Exception)
                {
                    return "Wrong BSR key format";
                }
                return "Unknown error";
            }
        }

        public bool Cleanup { get; set; }

        public CustomLevelViewModel(CustomLevel customLevel)
        {
            this.customLevel = customLevel;
        }
    }
}
