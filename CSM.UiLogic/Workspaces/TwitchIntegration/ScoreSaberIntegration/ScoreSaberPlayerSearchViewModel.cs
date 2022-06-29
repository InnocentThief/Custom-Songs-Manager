using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;

namespace CSM.UiLogic.Workspaces.TwitchIntegration.ScoreSaberIntegration
{
    public class ScoreSaberPlayerSearchViewModel
    {
        private ScoreSaberPlayerViewModel selectedPlayer;

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

        public RelayCommand AddPlayerCommand { get; }

        public RelayCommand CancelCommand { get; }

        public event EventHandler<PlayerSearchOnPlayerSelectedEventArgs> OnPlayerSelected;

        public event EventHandler OnCancel;

        public ScoreSaberPlayerSearchViewModel()
        {
            Players = new ObservableCollection<ScoreSaberPlayerViewModel>();
            AddPlayerCommand = new RelayCommand(AddPlayer, CanAddPlayer);
            CancelCommand = new RelayCommand(Cancel);
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
    }
}