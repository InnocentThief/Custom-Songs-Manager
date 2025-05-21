using CSM.Business.Interfaces;
using CSM.Framework.Extensions;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;
using CSM.UiLogic.ViewModels.Common.Leaderboard;
using CSM.UiLogic.ViewModels.Controls.SongSources;
using System.Collections.ObjectModel;
using System.IO;

namespace CSM.UiLogic.ViewModels.Controls.ScoreSaber
{
    internal class ScoreSaberControlViewModel : BaseViewModel, ISongSourceViewModel
    {
        #region Private fields

        private IRelayCommand? switchPlayerCommand;
        private bool playerSearchVisible;
        private ViewDefinition? selectedViewDefinition;
        private bool isSourceControl;

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

        public ObservableCollection<ViewDefinition> ViewDefinitions { get; } = [];

        public ViewDefinition? SelectedViewDefinition
        {
            get => selectedViewDefinition;
            set
            {
                if (value == selectedViewDefinition)
                    return;
                selectedViewDefinition = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanSaveViewDefinition));
                OnPropertyChanged(nameof(CanDeleteViewDefinition));

                if (isSourceControl)
                {
                    userConfigDomain!.Config!.PlaylistsConfig.LastSSSourceControlViewDefinitionName = selectedViewDefinition?.Name;
                }
                else
                {
                    userConfigDomain!.Config!.PlaylistsConfig.LastSSMainControlViewDefinitionName = selectedViewDefinition?.Name;
                }

                userConfigDomain.SaveUserConfig();
            }
        }

        public bool ShowViewDefinitions => ViewDefinitions.Count > 0;

        public bool CanSaveViewDefinition => SelectedViewDefinition != null;

        public bool CanDeleteViewDefinition => SelectedViewDefinition != null;

        #endregion

        public ScoreSaberControlViewModel(IServiceLocator serviceLocator, bool isSourceControl = false) : base(serviceLocator)
        {
            this.isSourceControl = isSourceControl;
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

        public override async Task<ViewDefinition?> SaveViewDefinitionAsync(Stream stream, SavableUiElement savableUiElement, string? name = null)
        {
            var newViewDefinition = await base.SaveViewDefinitionAsync(stream, savableUiElement, name);
            if (newViewDefinition != null)
            {
                ViewDefinitions.Add(newViewDefinition);
                SelectedViewDefinition = newViewDefinition;
                OnPropertyChanged(nameof(ShowViewDefinitions));
            }
            return newViewDefinition;
        }

        public override void DeleteViewDefinition(SavableUiElement savableUiElement, string name)
        {
            if (SelectedViewDefinition == null)
                return;
            base.DeleteViewDefinition(savableUiElement, name);
            ViewDefinitions.Remove(SelectedViewDefinition);
            SelectedViewDefinition = ViewDefinitions.FirstOrDefault();
            OnPropertyChanged(nameof(ShowViewDefinitions));
        }

        #region Helper methods

        private async Task LoadDataAsync(string playerId)
        {
            SetLoadingInProgress(true, "Loading player data...");

            var playerTask = scoreSaberService.GetPlayerProfileAsync(playerId);
            var scoresTask = scoreSaberService.GetPlayerScoresAsync(playerId, 1, 100);

            await Task.WhenAll(playerTask, scoresTask);

            var player = playerTask.Result;
            if (player != null)
            {
                Player = new ScoreSaberPlayerViewModel(ServiceLocator, player);
                OnPropertyChanged(nameof(Player));
            }

            var scoreResult = scoresTask.Result;
            if (scoreResult == null)
                return;
            // todo: cleanup dependency on ScoreSaberScoreViewModel
            Scores.Clear();
            foreach (var score in scoreResult.PlayerScores)
            {
                var scoreViewModel = new ScoreSaberScoreViewModel(ServiceLocator, score);
                Scores.Add(scoreViewModel);
            }

            var additionalRequestCount = scoreResult.Metadata.Total / 100 + 1;
            for (int i = 2; i <= additionalRequestCount; i++)
            {
                var additionalScoreResult = await scoreSaberService.GetPlayerScoresAsync(playerId, i, 100);
                if (additionalScoreResult == null)
                    continue;
                foreach (var score in additionalScoreResult.PlayerScores)
                {
                    var scoreViewModel = new ScoreSaberScoreViewModel(ServiceLocator, score);
                    Scores.Add(scoreViewModel);
                }
            }

            OnPropertyChanged(nameof(ScoreCount));

            // Load view definitions
            List<ViewDefinition> viewDefinitions;
            string? lastViewDefinition;
            if (isSourceControl)
            {
                viewDefinitions = await LoadViewDefinitionsAsync(SavableUiElement.SSSourceControl);
                lastViewDefinition = userConfigDomain!.Config?.PlaylistsConfig.LastSSSourceControlViewDefinitionName;
            }
            else
            {
                viewDefinitions = await LoadViewDefinitionsAsync(SavableUiElement.SSMainControl);
                lastViewDefinition = userConfigDomain!.Config?.PlaylistsConfig.LastSSMainControlViewDefinitionName;
            }

            ViewDefinitions.Clear();
            ViewDefinitions.AddRange(viewDefinitions);
            SelectedViewDefinition = ViewDefinitions.FirstOrDefault(vd => vd.Name == lastViewDefinition);
            OnPropertyChanged(nameof(ShowViewDefinitions));

            SetLoadingInProgress(false, string.Empty);
        }

        private void SwitchPlayer()
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
