using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.ScoreSaberIntegration
{
    public class ScoreSaberViewModel : ObservableObject
    {
        private int tabIndex ;

        public ScoreSaberPlayerBaseViewModel ScoreSaberSingle { get; }

        public ScoreSaberPlayerBaseViewModel ScoreSaberMultiple { get; }

        public int TabIndex
        {
            get => tabIndex;
            set
            {
                if (value == tabIndex) return;
                tabIndex = value;
                OnPropertyChanged();
            }
        }

        public ScoreSaberViewModel()
        {
            ScoreSaberSingle = new ScoreSaberSinglePlayerAnalysisViewModel();
            ScoreSaberMultiple = new ScoreSaberMultiplePlayersCompareViewModel();

            tabIndex = 1;
            // TODO: Set TabIndex to UserConfig.ScoreSaber.InitialTabIndex
        }

        public async Task AddPlayerFromTwitchAsync(string playername)
        {
            if (tabIndex == 0)
            {
                await ScoreSaberSingle.AddPlayerFromTwitchAsync(playername);
            }
            else
            {
                await ScoreSaberMultiple.AddPlayerFromTwitchAsync(playername);
            }
        }
    }
}