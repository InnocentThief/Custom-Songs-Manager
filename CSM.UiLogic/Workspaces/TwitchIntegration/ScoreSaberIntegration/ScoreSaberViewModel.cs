using CSM.DataAccess.Entities.Online.ScoreSaber;
using CSM.Services;
using CSM.UiLogic.Wizards;
using CSM.UiLogic.Workspaces.Common;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.TwitchIntegration.ScoreSaberIntegration
{
    public class ScoreSaberViewModel : ObservableObject
    {
        private ScoreSaberService scoreSaberService;
        private bool playerSearchVisible;

        public ScoreSaberPlayerViewModel Player1
        {
            get
            {
                if (Players.Count > 0) return Players[0];
                return null;
            }
        }

        public ScoreSaberPlayerViewModel Player2
        {
            get
            {
                if (Players.Count > 1) return Players[1];
                return null;
            }
        }

        public ScoreSaberPlayerViewModel Player3
        {
            get
            {
                if (Players.Count > 2) return Players[2];
                return null;
            }
        }

        public ScoreSaberPlayerViewModel Player4
        {
            get
            {
                if (Players.Count > 3) return Players[3];
                return null;
            }
        }

        public ScoreSaberPlayerViewModel Player5
        {
            get
            {
                if (Players.Count > 4) return Players[4];
                return null;
            }
        }

        public ScoreSaberPlayerViewModel Player6
        {
            get
            {
                if (Players.Count > 5) return Players[5];
                return null;
            }
        }

        public ObservableCollection<ScoreSaberPlayerViewModel> Players { get; }

        public RelayCommand AddPlayerCommand { get; }

        public ScoreSaberPlayerSearchViewModel PlayerSearch { get; private set; }

        public bool PlayerSearchVisible
        {
            get => playerSearchVisible;
            set
            {
                if (value == playerSearchVisible) return;
                playerSearchVisible = value;
                OnPropertyChanged();
            }
        }

        public ScoreSaberViewModel()
        {
            Players = new ObservableCollection<ScoreSaberPlayerViewModel>();
            PlayerSearch = new ScoreSaberPlayerSearchViewModel();
            PlayerSearch.OnPlayerSelected += PlayerSearch_OnPlayerSelected;
            PlayerSearch.OnCancel += PlayerSearch_OnCancel;

            scoreSaberService = new ScoreSaberService();

            AddPlayerCommand = new RelayCommand(ShowSearch, CanAddPlayer);
        }

        public async Task AddPlayerFromTwitchAsync(string playername)
        {
            if (CanAddPlayer())
            {
                var query = $"search={playername}";
                var players = await scoreSaberService.GetPlayersAsync(query);
                if (players != null && players.Players.Count == 1)
                {
                    if (!Players.Any(p => p.Id == players.Players.First().Id))
                    {
                        await AddPlayerAsync(players.Players.First());
                    }
                }
                else
                {
                    ShowSearch();
                    PlayerSearch.SearchTextPlayer = playername;
                }
                ChangePlayers();
            }
        }

        private void ShowSearch()
        {
            PlayerSearch.Clear();
            PlayerSearchVisible = true;
        }

        private async void PlayerSearch_OnPlayerSelected(object sender, PlayerSearchOnPlayerSelectedEventArgs e)
        {
            PlayerSearchVisible = false;
            if (!Players.Any(p => p.Id == e.Id))
            {
                var player = await scoreSaberService.GetFullPlayerInfoAsync(e.Id);
                await AddPlayerAsync(player);
                ChangePlayers();
            }
        }

        private async Task AddPlayerAsync(Player player)
        {
            var playerViewModel = new ScoreSaberPlayerViewModel(player)
            {
                Index = Players.Count
            };
            playerViewModel.OnRemove += PlayerViewModel_OnRemove;
            Players.Add(playerViewModel);
            AddPlayerCommand.NotifyCanExecuteChanged();
            await playerViewModel.LoadDataAsync();
            ChangePlayers();
        }

        private void PlayerViewModel_OnRemove(object sender, EventArgs e)
        {
            Players.Remove((ScoreSaberPlayerViewModel)sender);
            AddPlayerCommand.NotifyCanExecuteChanged();
            var index = 0;
            foreach (var player in Players)
            {
                player.Index = index;
                index++;
            }
            ChangePlayers();
        }

        private void PlayerSearch_OnCancel(object sender, EventArgs e)
        {
            PlayerSearchVisible = false;
        }

        private bool CanAddPlayer()
        {
            return Players.Count < 6;
        }

        private void ChangePlayers()
        {
            OnPropertyChanged(nameof(Player1));
            OnPropertyChanged(nameof(Player2));
            OnPropertyChanged(nameof(Player3));
            OnPropertyChanged(nameof(Player4));
            OnPropertyChanged(nameof(Player5));
            OnPropertyChanged(nameof(Player6));
        }
    }
}
