using CSM.DataAccess.Entities.Online;
using System.Collections.Generic;
using System.Linq;

namespace CSM.UiLogic.Workspaces.CustomLevels
{
    /// <summary>
    /// Represents a characteristic of a song that contains the difficulties.
    /// </summary>
    public class CustomLevelCharactersisticViewModel
    {
        private readonly IGrouping<string, Difficulty> characteristic;

        /// <summary>
        /// Gets the name of the characteristic.
        /// </summary>
        public string Name => characteristic.Key;

        /// <summary>
        /// Gets the grouping of the difficulties by charachteristic.
        /// </summary>
        public List<CustomLevelDifficultyViewModel> Difficulties => characteristic.Select(d => new CustomLevelDifficultyViewModel(d)).ToList();

        /// <summary>
        /// Initializes a new <see cref="CustomLevelCharactersisticViewModel"/>.
        /// </summary>
        /// <param name="characteristic">Characteristic name with the difficulty list.</param>
        public CustomLevelCharactersisticViewModel(IGrouping<string, Difficulty> characteristic)
        {
            this.characteristic = characteristic;
        }
    }
}