using CSM.DataAccess.Entities.Online.ScoreSaber;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;

namespace CSM.UiLogic.Workspaces.TwitchIntegration.ScoreSaberIntegration
{
    public class ScoreSaberPlayerViewModel : ObservableObject
    {
        #region Private fields

        private Player player;
        private int index;

        #endregion

        #region Public Properties

        public string Id => player.Id;

        public string Name => player.Name;

        public string Country => player.Country;

        public string ProfilePicture => player.ProfilePicture;

        public string Rank => $"#{player.Rank}";

        public string CountryRank => $"#{player.CountryRank}";

        public string PP => $"{player.PP} pp";

        public int TotalPlayCount => player.ScoreStats.TotalPlayCount;

        public int TotalRankedPlayCount => player.ScoreStats.RankedPlayCount;

        public string AverageRankedAccuracy => $"{Math.Round(player.ScoreStats.AverageRankedAccuracy, 2)}%";

        public int Index
        {
            get => index;
            set
            {
                if (value == index) return;
                index = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand RemoveCommand { get; }

        #endregion

        public event EventHandler OnRemove;

        public ScoreSaberPlayerViewModel(Player player)
        {
            this.player = player;
            RemoveCommand = new RelayCommand(Remove);
        }

        #region Helper methods

        private void Remove()
        {
            OnRemove?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}