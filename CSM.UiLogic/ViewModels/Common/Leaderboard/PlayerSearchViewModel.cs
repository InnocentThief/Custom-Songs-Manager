using System.Collections.ObjectModel;
using CSM.Business.Interfaces;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;

namespace CSM.UiLogic.ViewModels.Common.Leaderboard
{
    internal class PlayerSearchViewModel : BaseViewModel
    {
        #region Private fields

        private string searchText = string.Empty;
        private BasePlayerViewModel? selectedSearchResult;
        public IRelayCommand? searchCommand, cancelCommand, switchToPlayerCommand;

        private readonly LeaderboardSearchType leaderboardSearchType;
        private readonly IBeatLeaderService beatLeaderService;
        private readonly IScoreSaberService scoresaberService;

        #endregion

        #region Properties

        public IRelayCommand? SearchCommand => searchCommand ??= CommandFactory.CreateFromAsync(SearchAsync, CanSearch);

        public IRelayCommand? CancelCommand => cancelCommand ??= CommandFactory.Create(Cancel, CanCancel);

        public IRelayCommand? SwitchToPlayerCommand => switchToPlayerCommand ??= CommandFactory.Create(SwitchToPlayer, CanSwitchToPlayer);

        public string SearchText
        {
            get => searchText;
            set
            {
                if (searchText == value)
                    return;
                searchText = value;
                OnPropertyChanged();
                SearchCommand?.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<BasePlayerViewModel> SearchResults { get; } = [];

        public BasePlayerViewModel? SelectedSearchResult
        {
            get => selectedSearchResult;
            set
            {
                if (selectedSearchResult == value)
                    return;
                selectedSearchResult = value;
                OnPropertyChanged();
                SwitchToPlayerCommand?.RaiseCanExecuteChanged();
            }
        }

        #endregion

        public event EventHandler<SearchResultEventArgs>? SearchResultSelected;

        public PlayerSearchViewModel(IServiceLocator serviceLocator, LeaderboardSearchType leaderboardSearchType) : base(serviceLocator)
        {
            this.leaderboardSearchType = leaderboardSearchType;
            beatLeaderService = serviceLocator.GetService<IBeatLeaderService>();
            scoresaberService = serviceLocator.GetService<IScoreSaberService>();
        }

        public async Task SearchAsync()
        {
            if (leaderboardSearchType == LeaderboardSearchType.BeatLeader)
            {
                var searchResult = await beatLeaderService.GetPlayersAsync(SearchText);
                if (searchResult != null)
                {
                    SearchResults.Clear();
                    foreach (var player in searchResult.Data)
                    {
                        SearchResults.Add(new BeatLeaderPlayerViewModel(ServiceLocator, player));
                    }
                }
            }
            else
            {
                var searchResult = await scoresaberService.GetPlayersAsync(SearchText);
                if (searchResult != null)
                {
                    SearchResults.Clear();
                    foreach (var player in searchResult.Players)
                    {
                        SearchResults.Add(new ScoreSaberPlayerViewModel(ServiceLocator, player));
                    }
                }
            }
        }

        #region Helper methods

        private bool CanSearch()
        {
            return !string.IsNullOrEmpty(SearchText);
        }

        private void Cancel()
        {
            SearchResultSelected?.Invoke(this, new SearchResultEventArgs(null));
        }

        private bool CanCancel()
        {
            return true;
        }

        private void SwitchToPlayer()
        {
            if (SelectedSearchResult == null)
                return;
            SearchResultSelected?.Invoke(this, new SearchResultEventArgs(SelectedSearchResult.Id));
        }

        private bool CanSwitchToPlayer()
        {
            return SelectedSearchResult != null;
        }

        #endregion
    }
}
