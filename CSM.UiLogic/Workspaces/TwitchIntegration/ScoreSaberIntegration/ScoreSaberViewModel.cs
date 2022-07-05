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
                        AddPlayer(players.Players.First());
                    }
                }
                else
                {
                    ShowSearch();
                    PlayerSearch.SearchTextPlayer = playername;
                }
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
                AddPlayer(player);
            }
        }

        private void AddPlayer(Player player)
        {
            var playerViewModel = new ScoreSaberPlayerViewModel(player)
            {
                Index = Players.Count
            };
            playerViewModel.OnRemove += PlayerViewModel_OnRemove;
            Players.Add(playerViewModel);
            AddPlayerCommand.NotifyCanExecuteChanged();
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
        }

        private void PlayerSearch_OnCancel(object sender, EventArgs e)
        {
            PlayerSearchVisible = false;
        }

        private bool CanAddPlayer()
        {
            return Players.Count < 6;
        }
    }
}
