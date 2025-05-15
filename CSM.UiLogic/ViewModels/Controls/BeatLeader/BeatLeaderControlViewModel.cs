using System.Collections.ObjectModel;
using CSM.Business.Interfaces;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;
using CSM.UiLogic.ViewModels.Common.Leaderboard;

namespace CSM.UiLogic.ViewModels.Controls.BeatLeader
{
    internal class BeatLeaderControlViewModel : BaseViewModel
    {
        #region Private fields

        private IRelayCommand? switchPlayerCommand;
        private bool playerSearchVisible;

        private readonly IBeatLeaderService beatLeaderService;
        private readonly IUserConfigDomain userConfigDomain;

        #endregion

        #region Properties

        public IRelayCommand? SwichPlayerCommand => switchPlayerCommand ??= CommandFactory.Create(SwitchPlayer, CanSwitchPlayer);

        public BeatLeaderPlayerViewModel? Player { get; private set; }

        public ObservableCollection<BeatLeaderScoreViewModel> Scores { get; } = [];

        public string ScoreCount
        {
            get
            {
                if (Scores.Count == 0)
                    return "No score on record";
                if (Scores.Count == 1)
                    return "1 score";
                return $"{Scores.Count} scores";
            }
        }

        public bool PlayerSearchVisible
        {
            get => playerSearchVisible;
            set
            {
                if (value == playerSearchVisible)
                    return;
                playerSearchVisible = value;
                OnPropertyChanged();
            }
        }

        public PlayerSearchViewModel PlayerSearch { get; }

        #endregion

        public BeatLeaderControlViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
            beatLeaderService = serviceLocator.GetService<IBeatLeaderService>();
            userConfigDomain = serviceLocator.GetService<IUserConfigDomain>();

            PlayerSearch = new PlayerSearchViewModel(serviceLocator, LeaderboardSearchType.BeatLeader);
            PlayerSearch.SearchResultSelected += OnPlayerSearchResultSelected;
        }

        internal async Task LoadAsync(bool refresh)
        {
            if (Player != null && !refresh)
                return;

            var playerId = userConfigDomain.Config?.LeaderboardsConfig.BeatLeaderUserId;
            if (string.IsNullOrEmpty(playerId))
                return;

            await LoadDataAsync(playerId);

        }

        #region Helper methods

        private async Task LoadDataAsync(string playerId)
        {
            SetLoadingInProgress(true, "Loading player data...");

            var playerExists = await beatLeaderService.PlayerExistsAsync(playerId);
            if (!playerExists)
            {
                //todo: show message for unknown player
            }

            var playerTask = beatLeaderService.GetPlayerProfileAsync(playerId);
            var scoresTask = beatLeaderService.GetPlayerScoresAsync(playerId, 1, 100);

            await Task.WhenAll(playerTask, scoresTask);

            var player = playerTask.Result;
            if (player != null)
            {
                Player = new BeatLeaderPlayerViewModel(ServiceLocator, player);
                OnPropertyChanged(nameof(Player));
            }

            var scoreResult = scoresTask.Result;
            if (scoreResult == null)
                return;
            // todo: cleanup dependency on BeatLeaderScoreViewModel
            Scores.Clear();
            foreach (var score in scoreResult.Data)
            {
                var scoreViewModel = new BeatLeaderScoreViewModel(ServiceLocator, score);
                Scores.Add(scoreViewModel);
            }

            var additionalRequestCount = scoreResult.Metadata.Total / 100+1;
            for (int i = 2; i <= additionalRequestCount; i++)
            {
                var additionScoreResult = await beatLeaderService.GetPlayerScoresAsync(playerId, i, 100);
                if (additionScoreResult == null)
                    continue;
                foreach (var score in additionScoreResult.Data)
                {
                    var scoreViewModel = new BeatLeaderScoreViewModel(ServiceLocator, score);
                    Scores.Add(scoreViewModel);
                }
            }

            OnPropertyChanged(nameof(ScoreCount));

            SetLoadingInProgress(false, string.Empty);
        }

        public void SwitchPlayer()
        {
            PlayerSearchVisible = true;

        }

        private bool CanSwitchPlayer()
        {
            return true;
        }

        private async void OnPlayerSearchResultSelected(object? sender, SearchResultEventArgs e)
        {
            if (e.PlayerId == null)
                return;

            PlayerSearchVisible = false;
            await LoadDataAsync(e.PlayerId);
        }

        #endregion
    }
}
