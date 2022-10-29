using CSM.DataAccess.Entities.Online.ScoreSaber;
using CSM.Framework.Extensions;
using CSM.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.ScoreSaberIntegration
{
    public class ScoreSaberMultiplePlayersCompareViewModel : ScoreSaberPlayerBaseViewModel
    {
        #region Public Properties

        public ScoreSaberPlayerViewModel Player1
        {
            get
            {
                if (Players.Count > 0) return Players[0];
                return null;
            }
        }

        public ScoreSaberPlayerViewModel Player2
        {
            get
            {
                if (Players.Count > 1) return Players[1];
                return null;
            }
        }

        public ScoreSaberPlayerViewModel Player3
        {
            get
            {
                if (Players.Count > 2) return Players[2];
                return null;
            }
        }

        public ScoreSaberPlayerViewModel Player4
        {
            get
            {
                if (Players.Count > 3) return Players[3];
                return null;
            }
        }

        public ScoreSaberPlayerViewModel Player5
        {
            get
            {
                if (Players.Count > 4) return Players[4];
                return null;
            }
        }

        public ScoreSaberPlayerViewModel Player6
        {
            get
            {
                if (Players.Count > 5) return Players[5];
                return null;
            }
        }

        public ObservableCollection<ScoreSaberPlayerViewModel> Players { get; }

        public ObservableCollection<ScoreSaberSongViewModel> Songs { get; }

        #endregion

        public ScoreSaberMultiplePlayersCompareViewModel()
        {
            Players = new ObservableCollection<ScoreSaberPlayerViewModel>();
            Songs = new ObservableCollection<ScoreSaberSongViewModel>();
        }

        #region Helper methods

        private void PlayerViewModel_OnRemove(object sender, EventArgs e)
        {
            Players.Remove((ScoreSaberPlayerViewModel)sender);
            AddPlayerCommand.NotifyCanExecuteChanged();
            var index = 0;
            foreach (var player in Players)
            {
                player.Index = index;
                index++;
            }
            ChangePlayers();
        }

        private async Task AddPlayerAsync(Player player)
        {
            var playerViewModel = new ScoreSaberPlayerViewModel(player)
            {
                Index = Players.Count
            };
            playerViewModel.OnRemove += PlayerViewModel_OnRemove;
            Players.Add(playerViewModel);
            AddPlayerCommand.NotifyCanExecuteChanged();
            await playerViewModel.LoadDataAsync();
            ChangePlayers();
            AddSongsForPlayer(playerViewModel);
        }

        private void ChangePlayers()
        {
            OnPropertyChanged(nameof(Player1));
            OnPropertyChanged(nameof(Player2));
            OnPropertyChanged(nameof(Player3));
            OnPropertyChanged(nameof(Player4));
            OnPropertyChanged(nameof(Player5));
            OnPropertyChanged(nameof(Player6));
        }

        protected override bool CanAddPlayer()
        {
            return Players.Count < 6;
        }

        protected override async void PlayerSearch_OnPlayerSelected(object sender, PlayerSearchOnPlayerSelectedEventArgs e)
        {
            PlayerSearchVisible = false;
            if (!Players.Any(p => p.Id == e.Id))
            {
                var player = await ScoreSaberService.GetFullPlayerInfoAsync(e.Id);
                await AddPlayerAsync(player);
                ChangePlayers();
            }
        }

        public override async Task AddPlayerFromTwitchAsync(string playername)
        {
            var query = $"search={playername}";
            var players = await ScoreSaberService.GetPlayersAsync(query);
            if (players != null && players.Players.Count == 1)
            {
                if (!Players.Any(p => p.Id == players.Players.First().Id))
                {
                    await AddPlayerAsync(players.Players.First());
                }
            }
            else
            {
                ShowSearch();
                PlayerSearch.SearchTextPlayer = playername;
            }
            ChangePlayers();
        }

        #endregion

        private void AddSongsForPlayer(ScoreSaberPlayerViewModel scoreSaberPlayerViewModel)
        {
            return;
            // Update extisting song view models
            //var existingHashes = Songs.Select(s => s.SongHash).ToList();
            //var existingSongs = scoreSaberPlayerViewModel.Scores.Where(s => existingHashes.Contains(s.PlayerScore.Leaderboard.SongHash));
            //foreach (var existingSong in existingSongs)
            //{
            //    var song = Songs.Single(s => s.SongHash == existingSong.PlayerScore.Leaderboard.SongHash);
            //    song.AddPlayer(scoreSaberPlayerViewModel.Index, 4711);
            //}

            // Add all others
            //var newSongs = scoreSaberPlayerViewModel.Scores.Where(s => !existingHashes.Contains(s.PlayerScore.Leaderboard.SongHash));
            //Songs.AddRange(newSongs.Select(s => new ScoreSaberSongViewModel(s.PlayerScore.Leaderboard)));
        }
    }
}