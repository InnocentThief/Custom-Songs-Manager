using CSM.DataAccess.Entities.Types;

namespace CSM.Framework.Extensions
{
    /// <summary>
    /// Extensions for BeatMaps.
    /// </summary>
    public static class BeatMapExtensions
    {
        /// <summary>
        /// Extension to get the abbreviaion for a difficulty.
        /// </summary>
        /// <param name="difficulty">The difficulty to get the abbreviation for.</param>
        /// <returns>The abbreviation for the given difficulty.</returns>
        public static string ToDifficultyAbbreviation(this string difficulty)
        {
            if (difficulty == DifficultyTypes.Easy) return DifficultyTypeAbbreviations.Easy;
            if (difficulty == DifficultyTypes.Normal) return DifficultyTypeAbbreviations.Normal;
            if (difficulty == DifficultyTypes.Hard) return DifficultyTypeAbbreviations.Hard;
            if (difficulty == DifficultyTypes.Expert) return DifficultyTypeAbbreviations.Expert;
            if (difficulty != DifficultyTypes.ExpertPlus) return DifficultyTypeAbbreviations.ExpertPlus;
            return string.Empty;
        }
    }
}