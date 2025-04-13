using CSM.DataAccess.UserConfiguration;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Controls.Settings
{
    internal class PlaylistsSettingsViewModel(IServiceLocator serviceLocator, UserConfig userConfig) : BaseViewModel(serviceLocator)
    {
        private bool customLevelsSourceAvailable = userConfig.PlaylistsConfig.SourceAvailability.HasFlag(PlaylistsSourceAvailability.CustomLevels);
        private bool playlistsAvailable = userConfig.PlaylistsConfig.SourceAvailability.HasFlag(PlaylistsSourceAvailability.Playlists);
        private bool favouritesAvailable = userConfig.PlaylistsConfig.SourceAvailability.HasFlag(PlaylistsSourceAvailability.BeatSaberFavourites);
        private bool songSearchAvailable = userConfig.PlaylistsConfig.SourceAvailability.HasFlag(PlaylistsSourceAvailability.SongSearch);
        private bool songSuggestAvailable = userConfig.PlaylistsConfig.SourceAvailability.HasFlag(PlaylistsSourceAvailability.SongSuggest);

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

        private void UpdateSourceAvailability()
        {
            userConfig.PlaylistsConfig.SourceAvailability = PlaylistsSourceAvailability.None;
            if (customLevelsSourceAvailable) userConfig.PlaylistsConfig.SourceAvailability |= PlaylistsSourceAvailability.CustomLevels;
            if (playlistsAvailable) userConfig.PlaylistsConfig.SourceAvailability |= PlaylistsSourceAvailability.Playlists;
            if (favouritesAvailable) userConfig.PlaylistsConfig.SourceAvailability |= PlaylistsSourceAvailability.BeatSaberFavourites;
            if (songSearchAvailable) userConfig.PlaylistsConfig.SourceAvailability |= PlaylistsSourceAvailability.SongSearch;
            if (songSuggestAvailable) userConfig.PlaylistsConfig.SourceAvailability |= PlaylistsSourceAvailability.SongSuggest;
        }
    }
}
