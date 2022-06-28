using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.TwitchIntegration.ScoreSaberIntegration
{
    public class ScoreSaberViewModel
    {
        public ObservableCollection<ScoreSaberPlayerViewModel> Players { get; }

        public ScoreSaberViewModel()
        {
            Players = new ObservableCollection<ScoreSaberPlayerViewModel>();
            Players.Add(new ScoreSaberPlayerViewModel());
            Players.Add(new ScoreSaberPlayerViewModel());
            Players.Add(new ScoreSaberPlayerViewModel());
            Players.Add(new ScoreSaberPlayerViewModel());
        }
    }
}
