using CSM.DataAccess.Entities.Online.ScoreSaber;
using CSM.Framework.Extensions;
using CSM.Services;
using CSM.UiLogic.Properties;
using CSM.UiLogic.Wizards;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Telerik.Windows.Controls.TreeView;

namespace CSM.UiLogic.Workspaces.ScoreSaberIntegration
{
    public class ScoreSaberPlayerViewModel : ObservableObject
    {
        #region Private fields

        private readonly ListCollectionView itemsCollection;
        private readonly ObservableCollection<ScoreSaberPlayerScoreViewModel> itemsObservable;
        private Player player;
        private int index;
        private readonly ScoreSaberService scoreSaberService;
        private bool isLoading;

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

        public decimal TotalScore => player.ScoreStats.TotalScore;

        public decimal TotalRankedScore => player.ScoreStats.TotalRankedScore;

        public int ReplaysWatched => player.ScoreStats.ReplaysWatched;

        public string AverageRankedAccuracy => $"{Math.Round(player.ScoreStats.AverageRankedAccuracy, 2)}%";

        public ListCollectionView Scores => itemsCollection;

        public List<ScoreSaberPlayerScoreViewModel> ShowedScores { get; }

        public RelayCommand CopyUsernameCommand { get; }

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

        public bool IsLoading
        {
            get => isLoading;
            set
            {
                if (value == isLoading) return;
                isLoading = value;
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
            CopyUsernameCommand = new RelayCommand(CopyUsername);
            RankHistory = new ObservableCollection<RankDataPoint>();
            scoreSaberService = new ScoreSaberService();
            itemsObservable = new ObservableCollection<ScoreSaberPlayerScoreViewModel>();
            itemsCollection = DefaultSort();

            //Scores = new ObservableCollection<ScoreSaberPlayerScoreViewModel>();
            ShowedScores = new List<ScoreSaberPlayerScoreViewModel>();
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

        public void CopyUsername()
        {
            try
            {
                Clipboard.SetText(Name);
            }
            catch (Exception)
            {
                var messageBoxViewModel = new MessageBoxViewModel(Resources.OK, MessageBoxButtonColor.Default, String.Empty, MessageBoxButtonColor.Default)
                {
                    Title = Resources.ScoreSaber_CopyUsername_Error_Title,
                    Message = Resources.ScoreSaber_CopyUsername_Error_Message,
                    MessageBoxType = DataAccess.Entities.Types.MessageBoxTypes.Information
                };
                MessageBoxController.Instance().ShowMessageBox(messageBoxViewModel);
            }
        }

        private async Task LoadSongsAsync()
        {
            IsLoading = true;
            var scores = await scoreSaberService.GetPlayerScoresAsync(player.Id);
            itemsObservable.AddRange(scores.Select(s => new ScoreSaberPlayerScoreViewModel(s)));
            IsLoading = false;
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

        private ListCollectionView DefaultSort()
        {
            var collection = new ListCollectionView(itemsObservable);
            collection.SortDescriptions.Add(new SortDescription("TimeSet", ListSortDirection.Descending));
            return collection;
        }

        #endregion
    }
}