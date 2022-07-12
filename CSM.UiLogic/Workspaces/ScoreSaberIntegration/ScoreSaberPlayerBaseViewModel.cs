using CSM.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.ScoreSaberIntegration
{
    public abstract class ScoreSaberPlayerBaseViewModel : ObservableObject
    {
        private bool playerSearchVisible;

        protected ScoreSaberService ScoreSaberService { get; }

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

        public RelayCommand AddPlayerCommand { get; }

        protected ScoreSaberPlayerBaseViewModel()
        {
            PlayerSearch = new ScoreSaberPlayerSearchViewModel();
            PlayerSearch.OnPlayerSelected += PlayerSearch_OnPlayerSelected;
            PlayerSearch.OnCancel += PlayerSearch_OnCancel;

            ScoreSaberService = new ScoreSaberService();

            AddPlayerCommand = new RelayCommand(ShowSearch, CanAddPlayer);
        }

        public abstract Task AddPlayerFromTwitchAsync(string playername);

        protected abstract bool CanAddPlayer();

        protected abstract void PlayerSearch_OnPlayerSelected(object sender, PlayerSearchOnPlayerSelectedEventArgs e);

        private void PlayerSearch_OnCancel(object sender, EventArgs e)
        {
            PlayerSearchVisible = false;
        }

        protected void ShowSearch()
        {
            PlayerSearch.Clear();
            PlayerSearchVisible = true;
        }
    }
}