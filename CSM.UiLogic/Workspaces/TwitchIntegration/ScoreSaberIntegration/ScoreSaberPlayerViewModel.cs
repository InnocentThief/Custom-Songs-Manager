using CSM.DataAccess.Entities.Online.ScoreSaber;
using CSM.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.TwitchIntegration.ScoreSaberIntegration
{
    public class ScoreSaberPlayerViewModel : ObservableObject
    {
        #region Private fields

        private Player player;
        private int index;
        private ScoreSaberService scoreSaberService;

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

        public ObservableCollection<RankDataPoint> RankHistory { get; }



        public RelayCommand RemoveCommand { get; }

        public AsyncRelayCommand RefreshCommand { get; }

        #endregion

        public event EventHandler OnRemove;

        public ScoreSaberPlayerViewModel(Player player)
        {
            this.player = player;
            RemoveCommand = new RelayCommand(Remove);
            RefreshCommand = new AsyncRelayCommand(RefreshAsync);
            RankHistory = new ObservableCollection<RankDataPoint>();
            scoreSaberService = new ScoreSaberService();
        }

        public async Task LoadDataAsync()
        {
            await LoadSongsAsync();
            GenerateHistory();
        }

        #region Helper methods

        private void Remove()
        {
            OnRemove?.Invoke(this, EventArgs.Empty);
        }

        private async Task RefreshAsync()
        {
            player = await scoreSaberService.GetFullPlayerInfoAsync(player.Id);
            await LoadDataAsync();
        }

        private async Task LoadSongsAsync()
        {

        }

        private void GenerateHistory()
        {
            RankHistory.Clear();
            var histories = player.Histories.Split(',');
            var dayIndex = 48;
            foreach (var history in histories)
            {
                var rankHistory = new RankDataPoint
                {
                    Day = dayIndex != 0 ? $"{dayIndex + 2} days ago" : "yesterday",
                    Rank = string.IsNullOrWhiteSpace(history) ? null : int.Parse(history)
                };
                dayIndex--;
                RankHistory.Add(rankHistory);
            }
            RankHistory.Add(new RankDataPoint
            {
                Day = "today",
                Rank = player.Rank
            });
        }

        #endregion
    }
}