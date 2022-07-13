using System.Linq;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.ScoreSaberIntegration
{
    public class ScoreSaberSinglePlayerAnalysisViewModel : ScoreSaberPlayerBaseViewModel
    {
        private ScoreSaberPlayerViewModel scoreSaberPlayer;

        public ScoreSaberPlayerViewModel Player
        {
            get => scoreSaberPlayer;
            set
            {
                if (value == scoreSaberPlayer) return;
                scoreSaberPlayer = value;
                OnPropertyChanged();
            }
        }

        public override async Task AddPlayerFromTwitchAsync(string playername)
        {
            var query = $"search={playername}";
            var players = await ScoreSaberService.GetPlayersAsync(query);
            if (players != null && players.Players.Count == 1)
            {
                var player = await ScoreSaberService.GetFullPlayerInfoAsync(players.Players.First().Id);
                Player = new ScoreSaberPlayerViewModel(player);
                await Player.LoadDataAsync();
            }
            else
            {
                ShowSearch();
                PlayerSearch.SearchTextPlayer = playername;
            }
        }

        protected override bool CanAddPlayer()
        {
            return true;
        }

        protected override async void PlayerSearch_OnPlayerSelected(object sender, PlayerSearchOnPlayerSelectedEventArgs e)
        {
            PlayerSearchVisible = false;
            var player = await ScoreSaberService.GetFullPlayerInfoAsync(e.Id);
            Player = new ScoreSaberPlayerViewModel(player);
            await Player.LoadDataAsync();
        }
    }
}
