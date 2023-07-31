using CSM.DataAccess.Entities.Offline;
using CSM.Framework.Configuration.UserConfiguration;
using CSM.UiLogic.Wizards;
using CSM.UiLogic.Workspaces.Common;
using CSM.UiLogic.Workspaces.Playlists;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using CSM.UiLogic.Properties;
using CSM.Framework.Converter;
using System.Windows;

namespace CSM.UiLogic.Workspaces.ScoreSaberIntegration
{
    public class ScoreSaberSinglePlayerAnalysisViewModel : ScoreSaberPlayerBaseViewModel
    {
        private ScoreSaberPlayerViewModel scoreSaberPlayer;

        public ScoreSaberPlayerViewModel Player
        {
            get => scoreSaberPlayer;
            set
            {
                if (value == scoreSaberPlayer) return;
                scoreSaberPlayer = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SearchButtonVisible));
            }
        }

        public bool SearchButtonVisible => Player == null;

        public RelayCommand CreatePlaylistCommand { get; }

        public ScoreSaberSinglePlayerAnalysisViewModel()
        {
            CreatePlaylistCommand = new RelayCommand(CreatePlaylist);
        }

        public override async Task AddPlayerFromTwitchAsync(string playername)
        {
            var query = $"search={playername}";
            var players = await ScoreSaberService.GetPlayersAsync(query);
            if (players != null && players.Players.Count == 1)
            {
                var player = await ScoreSaberService.GetFullPlayerInfoAsync(players.Players.First().Id);
                Player = new ScoreSaberPlayerViewModel(player);
                await Player.LoadDataAsync();
            }
            else
            {
                ShowSearch();
                PlayerSearch.SearchTextPlayer = playername;
            }
        }

        protected override bool CanAddPlayer()
        {
            return true;
        }

        protected override async void PlayerSearch_OnPlayerSelected(object sender, PlayerSearchOnPlayerSelectedEventArgs e)
        {
            PlayerSearchVisible = false;
            var player = await ScoreSaberService.GetFullPlayerInfoAsync(e.Id);
            Player = new ScoreSaberPlayerViewModel(player);
            await Player.LoadDataAsync();
        }

        private void CreatePlaylist()
        {
            if (Player != null && !Player.ShowedScores.Any())
            {
                foreach (ScoreSaberPlayerScoreViewModel item in Player.Scores)
                {
                    Player.ShowedScores.Add(item);
                }
            }

            if (Player != null && Player.ShowedScores.Any())
            {
                var playlistsPath = UserConfigManager.Instance.Config.PlaylistPaths.First().Path;
                var fileViewModel = new EditWindowNewFileOrFolderNameViewModel(Resources.Playlists_AddPlaylist_Caption, Resources.Playlists_AddPlaylist_Content, false);
                EditWindowController.Instance().ShowEditWindow(fileViewModel);
                if (fileViewModel.Continue)
                {
                    try
                    {
                        var defaultImageLocation = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Images\\CSM_Logo_400px.png");

                        var playlistPath = Path.Combine(playlistsPath, $"{fileViewModel.FileOrFolderName}.json");
                        var playlist = new Playlist
                        {
                            Path = playlistPath,
                            PlaylistAuthor = String.Empty,
                            PlaylistDescription = String.Empty,
                            PlaylistTitle = fileViewModel.FileOrFolderName,
                            CustomData = null,
                            Songs = new List<PlaylistSong>(),
                            Image = $"base64,{ImageConverter.StringFromBitmap(defaultImageLocation)}"
                        };

                        // Save to file
                        var options = new JsonSerializerOptions { WriteIndented = true };
                        var content = JsonSerializer.Serialize(playlist, options);
                        File.WriteAllText(playlistPath, content);

                        // Create view model
                        var playlistViewModel = new PlaylistViewModel(playlist);

                        foreach (var item in Player.ShowedScores)
                        {
                            var songToAdd = new AddSongToPlaylistEventArgs()
                            {
                                Hash = item.PlayerScore.Leaderboard.SongHash
                            };
                            playlistViewModel.AddPlaylistSong(songToAdd);
                        }

                    }
                    catch (Exception)
                    {
                        MessageBox.Show(Resources.Playlist_WrongFileName_Content, Resources.Playlist_WrongFileName_Caption);
                        return;
                    }
                }
            }
        }
    }
}
