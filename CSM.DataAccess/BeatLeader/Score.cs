using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.DataAccess.BeatLeader
{
    internal class Score
    {
        public Leaderboard Leaderboard { get; set; } = new();
        public int Rank { get; set; }
        public int BadCuts { get; set; }
        public int MissedNotes { get; set; }
        public bool FullCombo { get; set; }
    }
}
