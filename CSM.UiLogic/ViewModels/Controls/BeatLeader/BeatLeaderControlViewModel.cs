using System.Collections.ObjectModel;
using CSM.Business.Interfaces;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.ViewModels.Common.Leaderboard;

namespace CSM.UiLogic.ViewModels.Controls.BeatLeader
{
    internal class BeatLeaderControlViewModel : BaseViewModel
    {
        private readonly IBeatLeaderService beatLeaderService;
        private readonly IUserConfigDomain userConfigDomain;

        public BeatLeaderPlayerViewModel? Player { get; private set; }

        public ObservableCollection<BeatLeaderScoreViewModel> Scores { get; } = [];

        public BeatLeaderControlViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
            beatLeaderService = serviceLocator.GetService<IBeatLeaderService>();
            userConfigDomain = serviceLocator.GetService<IUserConfigDomain>();
        }

        internal async Task LoadAsync(bool refresh)
        {
            if (Player != null && !refresh)
                return;

            SetLoadingInProgress(true, "Loading player data...");

            var playerId = userConfigDomain.Config?.LeaderboardsConfig.BeatLeaderUserId;
            if (string.IsNullOrEmpty(playerId))
                return;

            await LoadData(playerId);

            SetLoadingInProgress(false, string.Empty);
        }

        private async Task LoadData(string playerId)
        {
            var playerExists = await beatLeaderService.PlayerExistsAsync(playerId);
            if (!playerExists)
            {
                //todo: show message for unknown player
            }

            var playerTask = beatLeaderService.GetPlayerProfileAsync(playerId);
            var scoresTask = beatLeaderService.GetPlayerScoresAsync(playerId);

            await Task.WhenAll(playerTask, scoresTask);

            var player = playerTask.Result;
            if (player != null)
            {
                Player = new BeatLeaderPlayerViewModel(ServiceLocator, player);
                OnPropertyChanged(nameof(Player));
            }

            var scores = scoresTask.Result;
            // todo: cleanup dependency on BeatLeaderScoreViewModel
            Scores.Clear();
            foreach (var score in scores)
            {
                //var scoreViewModel = new BeatLeaderScoreViewModel(ServiceLocator, score);
                //Scores.Add(scoreViewModel);
            }
        }
    }
}
