using CSM.DataAccess.BeatSaver;
using CSM.DataAccess.Common;

namespace CSM.UiLogic.ViewModels.Common.MapDetails
{
    internal class MapDifficultyViewModel
    {
        private readonly MapDifficulty difficulty;
        private readonly MapVersion version;

        public Characteristic Characteristic => difficulty.Characteristic;

        public Difficulty Difficulty => difficulty.Difficulty;

        public string Label => difficulty.Label;

        public string Stars
        {
            get  {
                // todo: show stars based on config
                if (difficulty.Stars == 0)
                    return string.Empty;

                return $"{Math.Round(difficulty.BlStars, 2).ToString()}*";
                //return Math.Round(difficulty.Stars, 2).ToString();
            }
        }

        public MapDifficultyViewModel(MapDifficulty difficulty, MapVersion version)
        {
            this.difficulty = difficulty;
            this.version = version;
        }


    }
}
