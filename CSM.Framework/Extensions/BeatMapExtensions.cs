using CSM.DataAccess.Entities.Types;

namespace CSM.Framework.Extensions
{
    public static class BeatMapExtensions
    {
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