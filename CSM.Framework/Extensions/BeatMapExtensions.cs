namespace CSM.Framework.Extensions
{
    public static class BeatMapExtensions
    {
        public static string ToDifficultyAbbreviation(this string difficulty)
        {
            switch (difficulty)
            {
                case "Easy":
                    return "E";
                case "Normal":
                    return "N";
                case "Hard":
                    return "H";
                case "Expert":
                    return "Ex";
                case "ExpertPlus":
                    return "Ex+";
                default:
                    return string.Empty;
            }
        }
    }
}