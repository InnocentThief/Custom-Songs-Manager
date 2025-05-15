namespace CSM.DataAccess.BeatLeader
{
    internal class Score
    {
        public Leaderboard Leaderboard { get; set; } = new();
        public double Accuracy { get; set; }
        public double PP { get; set; }
        public double Weight { get; set; }
        public string Modifiers { get; set; } = string.Empty;
        public int Rank { get; set; }
        public double FcAccuracy { get; set; }
        public int BadCuts { get; set; }
        public int MissedNotes { get; set; }
        public bool FullCombo { get; set; }
        public int Pauses { get; set; }
        public int Timepost { get; set; }
    }
}
