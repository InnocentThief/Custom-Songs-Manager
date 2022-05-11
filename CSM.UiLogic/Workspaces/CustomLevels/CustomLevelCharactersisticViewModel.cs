using CSM.DataAccess.Entities.Online;
using System.Collections.Generic;
using System.Linq;

namespace CSM.UiLogic.Workspaces.CustomLevels
{
    public class CustomLevelCharactersisticViewModel
    {
        private IGrouping<string, Difficulty> characteristic;

        public string Name => characteristic.Key;

        public List<CustomLevelDifficultyViewModel> Difficulties => characteristic.Select(d=> new CustomLevelDifficultyViewModel(d)).ToList();

        public CustomLevelCharactersisticViewModel(IGrouping<string, Difficulty> characteristic)
        {
            this.characteristic = characteristic;
        }
    }
}