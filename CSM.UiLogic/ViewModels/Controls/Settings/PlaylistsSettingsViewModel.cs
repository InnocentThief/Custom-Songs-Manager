using CSM.DataAccess.UserConfiguration;
using CSM.Framework.Extensions;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Helper;
using System.Collections.ObjectModel;

namespace CSM.UiLogic.ViewModels.Controls.Settings
{
    internal class PlaylistsSettingsViewModel : BaseViewModel
    {
        #region Private fields

        private bool customLevelsSourceAvailable;
        private bool playlistsAvailable;
        private bool favouritesAvailable;
        private bool songSearchAvailable;
        private bool songSuggestAvailable;
        private bool twitchAvailable;
        private bool beatLeaderAvailable;
        private bool scoreSaberAvailable;

        private readonly UserConfig userConfig;

        #endregion

        #region Properties

        public bool Available
        {
            get => userConfig.PlaylistsConfig.Available;
            set
            {
                if (value == userConfig.PlaylistsConfig.Available)
                    return;
                userConfig.PlaylistsConfig.Available = value;
                OnPropertyChanged();
            }
        }

        public string PlaylistsPath
        {
            get => userConfig.PlaylistsConfig.PlaylistPath.Path;
            set
            {
                if (value == userConfig.PlaylistsConfig.PlaylistPath.Path)
                    return;
                userConfig.PlaylistsConfig.PlaylistPath.Path = value;
                OnPropertyChanged();
            }
        }

        public bool CustomLevelsSourceAvailable
        {
            get => customLevelsSourceAvailable;
            set
            {
                if (value == customLevelsSourceAvailable)
                    return;
                customLevelsSourceAvailable = value;
                OnPropertyChanged();
                UpdateSourceAvailability();
            }
        }

        public bool PlaylistsSourceAvailable
        {
            get => playlistsAvailable;
            set
            {
                if (value == playlistsAvailable)
                    return;
                playlistsAvailable = value;
                OnPropertyChanged();
                UpdateSourceAvailability();
            }
        }

        public bool FavouritesAvailable
        {
            get => favouritesAvailable;
            set
            {
                if (value == favouritesAvailable)
                    return;
                favouritesAvailable = value;
                OnPropertyChanged();
                UpdateSourceAvailability();
            }
        }

        public bool SongSearchAvailable
        {
            get => songSearchAvailable;
            set
            {
                if (value == songSearchAvailable)
                    return;
                songSearchAvailable = value;
                OnPropertyChanged();
                UpdateSourceAvailability();
            }
        }

        public bool SongSuggestAvailable
        {
            get => songSuggestAvailable;
            set
            {
                if (value == songSuggestAvailable)
                    return;
                songSuggestAvailable = value;
                OnPropertyChanged();
                UpdateSourceAvailability();
            }
        }

        public bool TwitchAvailable
        {
            get => twitchAvailable;
            set
            {
                if (value == twitchAvailable)
                    return;
                twitchAvailable = value;
                OnPropertyChanged();
                UpdateSourceAvailability();
            }
        }

        public bool BeatLeaderAvailable
        {
            get => beatLeaderAvailable;
            set
            {
                if (value == beatLeaderAvailable)
                    return;
                beatLeaderAvailable = value;
                OnPropertyChanged();
                UpdateSourceAvailability();
            }
        }

        public bool ScoreSaberAvailable
        {
            get => scoreSaberAvailable;
            set
            {
                if (value == scoreSaberAvailable)
                    return;
                scoreSaberAvailable = value;
                OnPropertyChanged();
                UpdateSourceAvailability();
            }
        }

        public ObservableCollection<EnumWrapper<PlaylistsSourceAvailability>> SongSources { get; } = [];

        public EnumWrapper<PlaylistsSourceAvailability>? SelectedSongSource
        {
            get => SongSources.FirstOrDefault(s => s.Value == userConfig.PlaylistsConfig.DefaultSource);
            set
            {
                if (value == null || value == SelectedSongSource)
                    return;
                userConfig.PlaylistsConfig.DefaultSource = value.Value;
                OnPropertyChanged();
            }
        }

        #endregion

        public PlaylistsSettingsViewModel(IServiceLocator serviceLocator, UserConfig userConfig) : base(serviceLocator)
        {
            this.userConfig = userConfig;

            customLevelsSourceAvailable = userConfig.PlaylistsConfig.SourceAvailability.HasFlag(PlaylistsSourceAvailability.CustomLevels);
            playlistsAvailable = userConfig.PlaylistsConfig.SourceAvailability.HasFlag(PlaylistsSourceAvailability.Playlists);
            favouritesAvailable = userConfig.PlaylistsConfig.SourceAvailability.HasFlag(PlaylistsSourceAvailability.BeatSaberFavourites);
            songSearchAvailable = userConfig.PlaylistsConfig.SourceAvailability.HasFlag(PlaylistsSourceAvailability.SongSearch);
            songSuggestAvailable = userConfig.PlaylistsConfig.SourceAvailability.HasFlag(PlaylistsSourceAvailability.SongSuggest);
            twitchAvailable = userConfig.PlaylistsConfig.SourceAvailability.HasFlag(PlaylistsSourceAvailability.Twitch);
            beatLeaderAvailable = userConfig.PlaylistsConfig.SourceAvailability.HasFlag(PlaylistsSourceAvailability.BeatLeader);
            scoreSaberAvailable = userConfig.PlaylistsConfig.SourceAvailability.HasFlag(PlaylistsSourceAvailability.ScoreSaber);

            SongSources.AddRange(EnumWrapper<PlaylistsSourceAvailability>.GetValues(serviceLocator, PlaylistsSourceAvailability.None));
        }

        #region Helper methods

        private void UpdateSourceAvailability()
        {
            userConfig.PlaylistsConfig.SourceAvailability = PlaylistsSourceAvailability.None;
            if (customLevelsSourceAvailable) userConfig.PlaylistsConfig.SourceAvailability |= PlaylistsSourceAvailability.CustomLevels;
            if (playlistsAvailable) userConfig.PlaylistsConfig.SourceAvailability |= PlaylistsSourceAvailability.Playlists;
            if (favouritesAvailable) userConfig.PlaylistsConfig.SourceAvailability |= PlaylistsSourceAvailability.BeatSaberFavourites;
            if (songSearchAvailable) userConfig.PlaylistsConfig.SourceAvailability |= PlaylistsSourceAvailability.SongSearch;
            if (songSuggestAvailable) userConfig.PlaylistsConfig.SourceAvailability |= PlaylistsSourceAvailability.SongSuggest;
            if (twitchAvailable) userConfig.PlaylistsConfig.SourceAvailability |= PlaylistsSourceAvailability.Twitch;
            if (beatLeaderAvailable) userConfig.PlaylistsConfig.SourceAvailability |= PlaylistsSourceAvailability.BeatLeader;
            if (scoreSaberAvailable) userConfig.PlaylistsConfig.SourceAvailability |= PlaylistsSourceAvailability.ScoreSaber;
        }

        #endregion
    }
}
