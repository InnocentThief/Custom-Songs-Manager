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

        public AsyncRelayCommand AddPlayerCommand { get; }

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

            scoreSaberService = new ScoreSaberService();

            AddPlayerCommand = new AsyncRelayCommand(AddPlayer);



            //Players.Add(new ScoreSaberPlayerViewModel());
            //Players.Add(new ScoreSaberPlayerViewModel());
            //Players.Add(new ScoreSaberPlayerViewModel());
            //Players.Add(new ScoreSaberPlayerViewModel());
        }

        private async Task AddPlayer()
        {
            PlayerSearch = new ScoreSaberPlayerSearchViewModel();
            PlayerSearch.OnPlayerSelected += PlayerSearch_OnPlayerSelected;
            PlayerSearch.OnCancel += PlayerSearch_OnCancel;
            PlayerSearchVisible = true;
        }

        private async void PlayerSearch_OnPlayerSelected(object sender, PlayerSearchOnPlayerSelectedEventArgs e)
        {

            PlayerSearch.OnPlayerSelected -= PlayerSearch_OnPlayerSelected;
            PlayerSearch.OnCancel -= PlayerSearch_OnCancel;
            PlayerSearchVisible = false;
        }

        private void PlayerSearch_OnCancel(object sender, EventArgs e)
        {
            PlayerSearch.OnPlayerSelected -= PlayerSearch_OnPlayerSelected;
            PlayerSearch.OnCancel -= PlayerSearch_OnCancel;
            PlayerSearchVisible = false;
        }
    }
}
