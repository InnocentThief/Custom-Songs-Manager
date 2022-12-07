using CSM.Framework.Extensions;
using CSM.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.ScoreSaberIntegration
{
    public class ScoreSaberPlayerSearchViewModel : ObservableObject
    {
        #region Private fields

        private ScoreSaberPlayerViewModel selectedPlayer;
        private string searchTextPlayer;
        private ScoreSaberService scoreSaberService;

        #endregion

        public ObservableCollection<ScoreSaberPlayerViewModel> Players { get; }

        public ScoreSaberPlayerViewModel SelectedPlayer
        {
            get => selectedPlayer;
            set
            {
                if (value == selectedPlayer) return;
                selectedPlayer = value;
                AddPlayerCommand.NotifyCanExecuteChanged();
            }
        }

        public string SearchTextPlayer
        {
            get => searchTextPlayer;
            set
            {
                if (value == searchTextPlayer) return;
                searchTextPlayer = value;
                OnPropertyChanged();
                SearchCommand.NotifyCanExecuteChanged();
            }
        }

        public AsyncRelayCommand SearchCommand { get; }

        public RelayCommand AddPlayerCommand { get; }

        public RelayCommand CancelCommand { get; }

        public event EventHandler<PlayerSearchOnPlayerSelectedEventArgs> OnPlayerSelected;

        public event EventHandler OnCancel;

        public ScoreSaberPlayerSearchViewModel()
        {
            Players = new ObservableCollection<ScoreSaberPlayerViewModel>();
            SearchCommand = new AsyncRelayCommand(SearchAsync, CanSearch);
            AddPlayerCommand = new RelayCommand(AddPlayer, CanAddPlayer);
            CancelCommand = new RelayCommand(Cancel);

            scoreSaberService = new ScoreSaberService();
        }

        public void Clear()
        {
            Players.Clear();
            SearchTextPlayer = String.Empty;
        }

        public async Task SearchAsync()
        {
            Players.Clear();
            var query = $"search={searchTextPlayer}";
            var players = await scoreSaberService.GetPlayersAsync(query);
            if (players != null)
            {
                Players.AddRange(players.Players.Select(p => new ScoreSaberPlayerViewModel(p)));
            }
        }

        private void AddPlayer()
        {
            OnPlayerSelected?.Invoke(this, new PlayerSearchOnPlayerSelectedEventArgs() { Id = selectedPlayer.Id });
        }

        private bool CanAddPlayer()
        {
            return selectedPlayer != null;
        }

        private void Cancel()
        {
            OnCancel?.Invoke(this, EventArgs.Empty);
        }

        private bool CanSearch()
        {
            return !string.IsNullOrWhiteSpace(searchTextPlayer);
        }
    }
}