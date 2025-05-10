using System.Collections.ObjectModel;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.ViewModels.Common.Leaderboard;

namespace CSM.UiLogic.ViewModels.Controls.ScoreSaber
{
    internal class ScoreSaberControlViewModel : BaseViewModel
    {
        public ScoreSaberPlayerViewModel? Player { get; private set; }

        public ObservableCollection<ScoreSaberScoreViewModel> Scores { get; } = [];

        public ScoreSaberControlViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }
    }
}
