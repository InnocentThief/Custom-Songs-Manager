using CSM.Business.Core.SongCopy;
using CSM.Business.Core.SongSelection;
using CSM.Business.Interfaces;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;
using CSM.UiLogic.ViewModels.Common.Playlists;
using Microsoft.Extensions.Logging;
using System.Windows;

namespace CSM.UiLogic.ViewModels.Controls.SongSources
{
    internal class SongSuggestSourceViewModel : BaseViewModel, ISongSourceViewModel
    {
        #region Private fields

        private PlaylistViewModel? playlist;
        private bool isDirty;
        private bool useDefaultSettings = true;
        private string? playerId = string.Empty;
        private IRelayCommand? generateCommand, resetAdvancedSettingsCommand, saveAdvancedSettingsCommand, createPlaylistCommand, overwritePlaylistCommand, mergePlaylistCommand;
        private string? createPlaylistCommandText, overwritePlaylistCommandText, mergePlaylistCommandText;
        private ISongSuggestDomain? songSuggestDomain;

        private readonly ILogger<SongSuggestSourceViewModel> logger;
        private readonly ISongCopyDomain songCopyDomain;
        private readonly IUserConfigDomain userConfigDomain;

        #endregion

        #region Properties

        public IRelayCommand GenerateCommand => generateCommand ??= CommandFactory.CreateFromAsync(GenerateAsync, CanGenerate);

        public IRelayCommand ResetAdvancedSettingsCommand => resetAdvancedSettingsCommand ??= CommandFactory.Create(ResetAdvancedSettings, CanResetAdvancedSettings);

        public IRelayCommand SaveAdvancedSettingsCommand => saveAdvancedSettingsCommand ??= CommandFactory.Create(SaveAdvancedSettings, CanSaveAdvancedSettings);

        public IRelayCommand CreatePlaylistCommand => createPlaylistCommand ??= CommandFactory.Create(CreatePlaylist, CanCreatePlaylist);

        public IRelayCommand OverwritePlaylistCommand => overwritePlaylistCommand ??= CommandFactory.Create(OverwritePlaylist, CanOverwritePlaylist);

        public IRelayCommand MergePlaylistCommand => mergePlaylistCommand ??= CommandFactory.Create(MergePlaylist, CanMergePlaylist);

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

        public PlaylistViewModel? Playlist
        {
            get => playlist;
            set
            {
                if (playlist == value)
                    return;
                playlist = value;
                OnPropertyChanged();
            }
        }

        public bool HasResults => Playlist != null && Playlist.Songs.Count > 0;

        public bool IsDirty
        {
            get => isDirty;
            set
            {
                if (value == isDirty)
                    return;
                isDirty = value;
                OnPropertyChanged();
                ResetAdvancedSettingsCommand.RaiseCanExecuteChanged();
                SaveAdvancedSettingsCommand.RaiseCanExecuteChanged();
            }
        }

        public bool UseDefaultSettings
        {
            get => useDefaultSettings;
            set
            {
                if (useDefaultSettings == value)
                    return;
                useDefaultSettings = value;
                OnPropertyChanged();
            }
        }

        public string? PlayerId
        {
            get => playerId;
            set
            {
                if (playerId == value)
                    return;
                playerId = value;
                OnPropertyChanged();
            }
        }

        public bool IgnorePlayedAll
        {
            get => userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.IgnorePlayedAll;
            set
            {
                if (value == userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.IgnorePlayedAll)
                    return;
                userConfigDomain.Config.SongSuggestConfig.SongSuggestSettings.IgnorePlayedAll = value;
                IsDirty = true;
                OnPropertyChanged();
            }
        }

        public int IgnorePlayedDays
        {
            get => userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.IgnorePlayedDays;
            set
            {
                if (value == userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.IgnorePlayedDays)
                    return;
                userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.IgnorePlayedDays = value;
                IsDirty = true;
                OnPropertyChanged();
            }
        }

        public bool IgnoreNonImprovable
        {
            get => userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.IgnoreNonImprovable;
            set
            {
                if (value == userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.IgnoreNonImprovable)
                    return;
                userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.IgnoreNonImprovable = value;
                IsDirty = true;
                OnPropertyChanged();
            }
        }

        public int RequiredMatches
        {
            get => userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.RequiredMatches;
            set
            {
                if (value == userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.RequiredMatches)
                    return;
                userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.RequiredMatches = value;
                IsDirty = true;
                OnPropertyChanged();
            }
        }

        public bool UseLikedSongs
        {
            get => userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.UseLikedSongs;
            set
            {
                if (value == userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.UseLikedSongs)
                    return;
                userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.UseLikedSongs = value;
                IsDirty = true;
                OnPropertyChanged();
            }
        }

        public bool FillLikedSongs
        {
            get => userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.FillLikedSongs;
            set
            {
                if (value == userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.FillLikedSongs)
                    return;
                userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.FillLikedSongs = value;
                IsDirty = true;
                OnPropertyChanged();
            }
        }

        public bool UseLocalScores
        {
            get => userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.UseLocalScores;
            set
            {
                if (value == userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.UseLocalScores)
                    return;
                userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.UseLocalScores = value;
                IsDirty = true;
                OnPropertyChanged();
            }
        }

        public int ExtraSongs
        {
            get => userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.ExtraSongs;
            set
            {
                if (value == userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.ExtraSongs)
                    return;
                userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.ExtraSongs = value;
                IsDirty = true;
                OnPropertyChanged();
            }
        }

        public int PlaylistLength
        {
            get => userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.PlaylistLength;
            set
            {
                if (value == userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.PlaylistLength)
                    return;
                userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.PlaylistLength = value;
                IsDirty = true;
                OnPropertyChanged();
            }
        }

        // TODO: Leaderboard

        public int OriginSongCount
        {
            get => userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.OriginSongCount;
            set
            {
                if (value == userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.OriginSongCount)
                    return;
                userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.OriginSongCount = value;
                IsDirty = true;
                OnPropertyChanged();
            }
        }

        public double ModifierStyle
        {
            get => userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.FilterSettings.ModifierStyle;
            set
            {
                if (value == userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.FilterSettings.ModifierStyle)
                    return;
                userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.FilterSettings.ModifierStyle = value;
                IsDirty = true;
                OnPropertyChanged();
            }
        }

        public double ModifierOverweight
        {
            get => userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.FilterSettings.ModifierOverweight;
            set
            {
                if (value == userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.FilterSettings.ModifierOverweight)
                    return;
                userConfigDomain.Config!.SongSuggestConfig.SongSuggestSettings.FilterSettings.ModifierOverweight = value;
                IsDirty = true;
                OnPropertyChanged();
            }
        }

        #endregion

        public SongSuggestSourceViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
            logger = serviceLocator.GetService<ILogger<SongSuggestSourceViewModel>>();
            songCopyDomain = serviceLocator.GetService<ISongCopyDomain>();
            songCopyDomain.OnPlaylistSelectionChanged += SongCopyDomain_OnPlaylistSelectionChanged;
            userConfigDomain = serviceLocator.GetService<IUserConfigDomain>();

            createPlaylistCommandText = "Create new playlist in root with all songs (all filter will apply)";
        }

        public async Task LoadAsync()
        {
            SetLoadingInProgress(true, "Initializing song suggestion...");
            try
            {
                songSuggestDomain = ServiceLocator.GetService<ISongSuggestDomain>();
                await songSuggestDomain.InitializeAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error initializing song suggestion.");
            }
            finally
            {
                SetLoadingInProgress(false, string.Empty);
            }
        }

        #region Helper methods

        private async Task GenerateAsync()
        {
            if (songSuggestDomain == null)
                return;

            SetLoadingInProgress(true, "Generating song suggestions...");
            var playerIdToUse = string.IsNullOrWhiteSpace(playerId) ? null : playerId;
            await songSuggestDomain.GenerateSongSuggestionsAsync(playerIdToUse);
            var playlist = await songSuggestDomain.GetPlaylistAsync();
            if (playlist != null)
            {
                var playlistViewModel = new PlaylistViewModel(ServiceLocator, playlist, songSuggestDomain.GetPlaylistPath() ?? string.Empty, SongSelectionType.Right, true);
                await playlistViewModel.FetchDataAsync();
                Playlist = playlistViewModel;
                OnPropertyChanged(nameof(Playlist));
                OnPropertyChanged(nameof(HasResults));
            }
            SetLoadingInProgress(false, string.Empty);
        }

        private bool CanGenerate()
        {
            return true; // check for config?
        }

        private void ResetAdvancedSettings()
        {
            userConfigDomain.LoadOrCreateUserConfig();
            IsDirty = false;

            OnPropertyChanged(nameof(IgnorePlayedAll));
            OnPropertyChanged(nameof(IgnorePlayedDays));
            OnPropertyChanged(nameof(IgnoreNonImprovable));
            OnPropertyChanged(nameof(RequiredMatches));
            OnPropertyChanged(nameof(UseLikedSongs));
            OnPropertyChanged(nameof(FillLikedSongs));
            OnPropertyChanged(nameof(UseLocalScores));
            OnPropertyChanged(nameof(ExtraSongs));
            OnPropertyChanged(nameof(PlaylistLength));
            OnPropertyChanged(nameof(OriginSongCount));
            OnPropertyChanged(nameof(ModifierStyle));
            OnPropertyChanged(nameof(ModifierOverweight));
        }

        private bool CanResetAdvancedSettings()
        {
            return isDirty;
        }

        private void SaveAdvancedSettings()
        {
            userConfigDomain.SaveUserConfig();
            IsDirty = false;
        }

        private bool CanSaveAdvancedSettings()
        {
            return isDirty;
        }

        private void CreatePlaylist()
        {
            if (Playlist == null || Playlist.Songs.Count == 0)
            {
                MessageBox.Show("No songs to copy.", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var createPlaylistEventArgs = new CreatePlaylistEventArgs
            {
                PlaylistName = $"Suggested Songs {DateTime.Now:yyyy-MM-dd HH-mm-ss}",
                Songs = [.. Playlist.Songs.Select(x => x.Model)] // todo: only take filtered songs
            };
            songCopyDomain.CreatePlaylist(createPlaylistEventArgs);
        }

        private bool CanCreatePlaylist()
        {
            return songCopyDomain.SelectedPlaylist == null || songCopyDomain.SelectedPlaylist is PlaylistFolderViewModel;
        }

        private void OverwritePlaylist()
        {

        }

        private bool CanOverwritePlaylist()
        {
            return songCopyDomain.SelectedPlaylist != null && songCopyDomain.SelectedPlaylist is PlaylistViewModel;
        }

        private void MergePlaylist()
        {

        }

        private bool CanMergePlaylist()
        {
            return songCopyDomain.SelectedPlaylist != null && songCopyDomain.SelectedPlaylist is PlaylistViewModel;
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

            CreatePlaylistCommand.RaiseCanExecuteChanged();
            OverwritePlaylistCommand.RaiseCanExecuteChanged();
            MergePlaylistCommand.RaiseCanExecuteChanged();
        }

        #endregion
    }
}
