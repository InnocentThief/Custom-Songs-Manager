using CSM.Business.Interfaces;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;
using CSM.UiLogic.ViewModels.Common.Playlists;
using Microsoft.Extensions.Logging;

namespace CSM.UiLogic.ViewModels.Controls.SongSources
{
    internal class SongSuggestSourceViewModel(IServiceLocator serviceLocator) : BaseViewModel(serviceLocator), ISongSourceViewModel
    {
        #region Private fields

        private PlaylistViewModel? playlist;
        private bool isDirty;
        private bool useDefaultSettings = true;
        private string? playerId = string.Empty;
        private IRelayCommand? generateCommand, resetAdvancedSettingsCommand, saveAdvancedSettingsCommand;
        private ISongSuggestDomain? songSuggestDomain;

        private readonly ILogger<SongSuggestSourceViewModel> logger = serviceLocator.GetService<ILogger<SongSuggestSourceViewModel>>();
        private readonly IUserConfigDomain userConfigDomain = serviceLocator.GetService<IUserConfigDomain>();

        #endregion

        #region Properties

        public IRelayCommand GenerateCommand => generateCommand ??= CommandFactory.CreateFromAsync(GenerateAsync, CanGenerate);

        public IRelayCommand ResetAdvancedSettingsCommand => resetAdvancedSettingsCommand ??= CommandFactory.Create(ResetAdvancedSettings, CanResetAdvancedSettings);

        public IRelayCommand SaveAdvancedSettingsCommand => saveAdvancedSettingsCommand ??= CommandFactory.Create(SaveAdvancedSettings, CanSaveAdvancedSettings);

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
                var playlistViewModel = new PlaylistViewModel(ServiceLocator, playlist, "");
                await playlistViewModel.FetchDataAsync();
                Playlist = playlistViewModel;
                OnPropertyChanged(nameof(Playlist));
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

        #endregion
    }
}
