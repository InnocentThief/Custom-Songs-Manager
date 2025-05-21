namespace CSM.DataAccess.ScoreSaber
{
    internal class Score
    {
        public int Id { get; set; }
        public int Rank { get; set; }
        public int BaseScore { get; set; }
        public int ModifiedScore { get; set; }
        public double PP { get; set; }
        public double Weight { get; set; }
        public string Modifiers { get; set; } = string.Empty;
        public double Multiplier { get; set; }
        public int BadCuts { get; set; }
        public int MissedNotes { get; set; }
        public int MaxCombo { get; set; }
        public bool FullCombo { get; set; }
        public int HMD { get; set; }
        public bool HasReplay { get; set; }
        public DateTime TimeSet { get; set; }
        public string DeviceHmd { get; set; } = string.Empty;
        public string DeviceControllerLeft { get; set; } = string.Empty;
        public string DeviceControllerRight { get; set; } = string.Empty;
    }
}
