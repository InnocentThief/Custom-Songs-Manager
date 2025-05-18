using CSM.Business.Interfaces;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;
using CSM.UiLogic.ViewModels.Common.Leaderboard;
using CSM.UiLogic.ViewModels.Controls.SongSources;
using System.Collections.ObjectModel;

namespace CSM.UiLogic.ViewModels.Controls.ScoreSaber
{
    internal class ScoreSaberControlViewModel : BaseViewModel, ISongSourceViewModel
    {
        #region Private fields

        private IRelayCommand? switchPlayerCommand;
        private bool playerSearchVisible;

        private readonly IScoreSaberService scoreSaberService;
        private readonly IUserConfigDomain userConfigDomain;

        #endregion

        #region Properties

        public IRelayCommand? SwichPlayerCommand => switchPlayerCommand ??= CommandFactory.Create(SwitchPlayer, CanSwitchPlayer);

        public ScoreSaberPlayerViewModel? Player { get; private set; }

        public ObservableCollection<ScoreSaberScoreViewModel> Scores { get; } = [];

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

        public ScoreSaberControlViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
            scoreSaberService = serviceLocator.GetService<IScoreSaberService>();
            userConfigDomain = serviceLocator.GetService<IUserConfigDomain>();

            PlayerSearch = new PlayerSearchViewModel(serviceLocator, LeaderboardSearchType.ScoreSaber);
            PlayerSearch.SearchResultSelected += OnPlayerSearchResultSelected;
        }

        public async Task LoadAsync(bool refresh)
        {
            if (Player != null && !refresh)
                return;

            var playerId = userConfigDomain.Config?.LeaderboardsConfig.ScoreSaberUserId;
            if (string.IsNullOrEmpty(playerId))
                return;

            await LoadDataAsync(playerId);
        }

        #region Helper methods

        private async Task LoadDataAsync(string playerId)
        {
            SetLoadingInProgress(true, "Loading player data...");






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
            PlayerSearchVisible = false;

            if (e.PlayerId == null)
                return;
            await LoadDataAsync(e.PlayerId);
        }

        #endregion
    }
}
