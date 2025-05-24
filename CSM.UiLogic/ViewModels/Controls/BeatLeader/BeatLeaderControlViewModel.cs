using CSM.Business.Core.SongCopy;
using CSM.Business.Core;
using CSM.Business.Interfaces;
using CSM.DataAccess.UserConfiguration;
using CSM.Framework.Extensions;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;
using CSM.UiLogic.ViewModels.Common.Leaderboard;
using CSM.UiLogic.ViewModels.Common.Playlists;
using CSM.UiLogic.ViewModels.Controls.SongSources;
using System.Collections.ObjectModel;
using System.IO;
using CSM.DataAccess.Playlists;
using CSM.Business.Core.SongSelection;
using CSM.UiLogic.ViewModels.Controls.PlaylistsTree;
using System.Reflection;

namespace CSM.UiLogic.ViewModels.Controls.BeatLeader
{
    internal class BeatLeaderControlViewModel : BaseViewModel, ISongSourceViewModel
    {
        #region Private fields

        private IRelayCommand? switchPlayerCommand, createPlaylistCommand, overwritePlaylistCommand, mergePlaylistCommand;
        private bool playerSearchVisible;
        private ViewDefinition? selectedViewDefinition;
        private string? createPlaylistCommandText, overwritePlaylistCommandText, mergePlaylistCommandText;
        private BeatLeaderScoreViewModel? selectedScore;

        private readonly bool isSourceControl;
        private readonly IBeatLeaderService beatLeaderService;
        private readonly ISongCopyDomain songCopyDomain;
        private readonly ISongSelectionDomain songSelectionDomain;
        private readonly IUserConfigDomain userConfigDomain;

        #endregion

        #region Properties

        public IRelayCommand? SwichPlayerCommand => switchPlayerCommand ??= CommandFactory.Create(SwitchPlayer, CanSwitchPlayer);
        public IRelayCommand? CreatePlaylistCommand => createPlaylistCommand ??= CommandFactory.Create(CreatePlaylist, CanCreatePlaylist);
        public IRelayCommand? OverwritePlaylistCommand => overwritePlaylistCommand ??= CommandFactory.Create(OverwritePlaylist, CanOverwritePlaylist);
        public IRelayCommand? MergePlaylistCommand => mergePlaylistCommand ??= CommandFactory.Create(MergePlaylist, CanMergePlaylist);

        public BeatLeaderPlayerViewModel? Player { get; private set; }

        public ObservableCollection<BeatLeaderScoreViewModel> Scores { get; } = [];

        public BeatLeaderScoreViewModel? SelectedScore
        {
            get => selectedScore;
            set
            {
                if (value == selectedScore)
                    return;
                selectedScore = value;
                OnPropertyChanged();
                if (selectedScore != null)
                    songSelectionDomain.SetSongHash(selectedScore.Model.Leaderboard.Song.Hash, SongSelectionType.Right);
            }
        }

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
                    userConfigDomain!.Config!.PlaylistsConfig.LastBlSourceControlViewDefinitionName = selectedViewDefinition?.Name;
                }
                else
                {
                    userConfigDomain!.Config!.PlaylistsConfig.LastBlMainControlViewDefinitionName = selectedViewDefinition?.Name;
                }

                userConfigDomain.SaveUserConfig();
            }
        }

        public bool ShowViewDefinitions => ViewDefinitions.Count > 0;

        public bool CanSaveViewDefinition => SelectedViewDefinition != null;

        public bool CanDeleteViewDefinition => SelectedViewDefinition != null;

        public FilterMode FilterMode => userConfigDomain.Config?.FilterMode ?? FilterMode.PopUp;

        public string? CreatePlaylistCommandText
        {
            get => createPlaylistCommandText;
            set
            {
                if (createPlaylistCommandText == value)
                    return;
                createPlaylistCommandText = value;
                OnPropertyChanged();
            }
        }

        public string? OverwritePlaylistCommandText
        {
            get => overwritePlaylistCommandText;
            set
            {
                if (overwritePlaylistCommandText == value)
                    return;
                overwritePlaylistCommandText = value;
                OnPropertyChanged();
            }
        }

        public string? MergePlaylistCommandText
        {
            get => mergePlaylistCommandText;
            set
            {
                if (mergePlaylistCommandText == value)
                    return;
                mergePlaylistCommandText = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public BeatLeaderControlViewModel(IServiceLocator serviceLocator, bool isSourceControl = false) : base(serviceLocator)
        {
            this.isSourceControl = isSourceControl;
            beatLeaderService = serviceLocator.GetService<IBeatLeaderService>();
            songCopyDomain = serviceLocator.GetService<ISongCopyDomain>();
            songCopyDomain.OnPlaylistSelectionChanged += SongCopyDomain_OnPlaylistSelectionChanged;
            songSelectionDomain = serviceLocator.GetService<ISongSelectionDomain>();
            userConfigDomain = serviceLocator.GetService<IUserConfigDomain>();

            PlayerSearch = new PlayerSearchViewModel(serviceLocator, LeaderboardSearchType.BeatLeader);
            PlayerSearch.SearchResultSelected += OnPlayerSearchResultSelected;
        }

        public async Task LoadAsync(bool refresh)
        {
            if (Player != null && !refresh)
                return;

            var playerId = userConfigDomain.Config?.LeaderboardsConfig.BeatLeaderUserId;
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
            Scores.ForEach(s => s.CleanUpReferences());
            Scores.Clear();
            foreach (var score in scoreResult.Data)
            {
                var scoreViewModel = new BeatLeaderScoreViewModel(ServiceLocator, score);
                Scores.Add(scoreViewModel);
            }

            var additionalRequestCount = scoreResult.Metadata.Total / 100 + 1;
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

            // Load view definitions
            List<ViewDefinition> viewDefinitions;
            string? lastViewDefinition;
            if (isSourceControl)
            {
                viewDefinitions = await LoadViewDefinitionsAsync(SavableUiElement.BlSourceControl);
                lastViewDefinition = userConfigDomain!.Config?.PlaylistsConfig.LastBlSourceControlViewDefinitionName;
            }
            else
            {
                viewDefinitions = await LoadViewDefinitionsAsync(SavableUiElement.BlMainControl);
                lastViewDefinition = userConfigDomain!.Config?.PlaylistsConfig.LastBlMainControlViewDefinitionName;
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

        private void SongCopyDomain_OnPlaylistSelectionChanged(object? sender, Business.Core.SongCopy.PlaylistSelectionChangedEventArgs e)
        {
            if (e.Playlist == null)
            {
                CreatePlaylistCommandText = "Create new playlist in root with all songs (all filter will apply)";
                OnPropertyChanged(nameof(CreatePlaylistCommandText));
            }
            else if (e.Playlist is PlaylistFolderViewModel playlistFolderViewModel)
            {
                CreatePlaylistCommandText = $"Create new playlist in folder '{playlistFolderViewModel.Name}' with all songs (all filter will apply)";
                OnPropertyChanged(nameof(CreatePlaylistCommandText));
            }
            else if (e.Playlist is PlaylistViewModel playlistViewModel)
            {
                OverwritePlaylistCommandText = $"Overwrite playlist '{playlistViewModel.PlaylistTitle}' with all songs (all filter will apply)";
                OnPropertyChanged(nameof(OverwritePlaylistCommandText));
                MergePlaylistCommandText = $"Merge all songs (all filter will apply) with songs from playlist '{playlistViewModel.PlaylistTitle}'";
                OnPropertyChanged(nameof(MergePlaylistCommandText));
            }

            CreatePlaylistCommand?.RaiseCanExecuteChanged();
            OverwritePlaylistCommand?.RaiseCanExecuteChanged();
            MergePlaylistCommand?.RaiseCanExecuteChanged();
        }

        private void CreatePlaylist()
        {
            var editNewPlaylistName = new NewPlaylistViewModel(ServiceLocator, "Cancel", EditViewModelCommandColor.Default, "Create playlist", EditViewModelCommandColor.Default)
            {
                PlaylistName = $"BeatLeader {Player?.Name ?? string.Empty} {DateTime.Now:yyyy-MM-dd HH-mm-ss}"
            };
            UserInteraction.ShowWindow(editNewPlaylistName);
            if (editNewPlaylistName.Continue)
            {
                // todo: only take filtered songs
                var songs = Scores.Select(s => new Song
                {
                    Hash = s.Model.Leaderboard.Song.Hash,
                    LevelAuthorName = s.Model.Leaderboard.Song.Mapper,
                    SongName = s.Model.Leaderboard.Song.Name,
                    Difficulties =
                    [
                        new Difficulty
                    {
                        Characteristic = s.Model.Leaderboard.Difficulty.ModeName,
                        Name = s.Model.Leaderboard.Difficulty.DifficultyName
                    }
                    ]
                });

                var createPlaylistEventArgs = new CreatePlaylistEventArgs
                {
                    PlaylistName = editNewPlaylistName.PlaylistName,
                    Songs = [.. songs]
                };
                songCopyDomain.CreatePlaylist(createPlaylistEventArgs);
            }
        }

        private bool CanCreatePlaylist()
        {
            return songCopyDomain.SelectedPlaylist != null || songCopyDomain.SelectedPlaylist is PlaylistFolderViewModel;
        }
        private void OverwritePlaylist()
        {
            // todo: only take filtered songs
            var songs = Scores.Select(s => new Song
            {
                Hash = s.Model.Leaderboard.Song.Hash,
                LevelAuthorName = s.Model.Leaderboard.Song.Mapper,
                SongName = s.Model.Leaderboard.Song.Name,
                Difficulties =
                [
                    new Difficulty
                    {
                        Characteristic = s.Model.Leaderboard.Difficulty.ModeName,
                        Name = s.Model.Leaderboard.Difficulty.DifficultyName
                    }
                ]
            });

            var songCopyEventArgs = new SongCopyEventArgs
            {
                OverwritePlaylist = true,
                Songs = [.. songs]
            };
            songCopyDomain.CopySongs(songCopyEventArgs);
        }

        private bool CanOverwritePlaylist()
        {
            return songCopyDomain.SelectedPlaylist != null && songCopyDomain.SelectedPlaylist is PlaylistViewModel;
        }
        private void MergePlaylist()
        {
            // todo: only take filtered songs
            var songs = Scores.Select(s => new Song
            {
                Hash = s.Model.Leaderboard.Song.Hash,
                LevelAuthorName = s.Model.Leaderboard.Song.Mapper,
                SongName = s.Model.Leaderboard.Song.Name,
                Difficulties =
                [
                    new Difficulty
                    {
                        Characteristic = s.Model.Leaderboard.Difficulty.ModeName,
                        Name = s.Model.Leaderboard.Difficulty.DifficultyName
                    }
                ]
            });

            var songCopyEventArgs = new SongCopyEventArgs
            {
                Songs = [.. songs]
            };
            songCopyDomain.CopySongs(songCopyEventArgs);
        }

        private bool CanMergePlaylist()
        {
            return songCopyDomain.SelectedPlaylist != null && songCopyDomain.SelectedPlaylist is PlaylistViewModel;
        }

        #endregion
    }
}
