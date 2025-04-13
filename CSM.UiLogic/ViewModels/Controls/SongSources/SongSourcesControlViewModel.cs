using CSM.Business.Interfaces;
using CSM.DataAccess.UserConfiguration;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Controls.SongSources
{
    internal class SongSourcesControlViewModel : BaseViewModel
    {
        #region Private fields

        private SongSource songSource = SongSource.CustomLevels;

        private readonly UserConfig? userConfig;

        #endregion

        #region Properties

        public bool AnySourcesAvailable => CustomLevelsAvailable || PlaylistsAvailable || FavouritesAvailable || SearchAvailable || SongSuggestAvailable;

        public SongSource SongSource
        {
            get => songSource;
            set
            {
                if (value == songSource)
                    return;
                songSource = value;
                OnPropertyChanged();
            }
        }

        public bool CustomLevelsAvailable => userConfig?.PlaylistsConfig.SourceAvailability.HasFlag(PlaylistsSourceAvailability.CustomLevels) ?? false;

        public bool IsCustomLevelsSelected
        {
            get => SongSource == SongSource.CustomLevels;
            set
            {
                if (value)
                {
                    SongSource = SongSource.CustomLevels;
                    OnPropertyChanged();
                }
            }
        }

        public bool PlaylistsAvailable => userConfig?.PlaylistsConfig.SourceAvailability.HasFlag(PlaylistsSourceAvailability.Playlists) ?? false;

        public bool IsPlaylistsSelected
        {
            get => SongSource == SongSource.Playlists;
            set
            {
                if (value)
                {
                    SongSource = SongSource.Playlists;
                    OnPropertyChanged();
                }
            }
        }

        public bool FavouritesAvailable => userConfig?.PlaylistsConfig.SourceAvailability.HasFlag(PlaylistsSourceAvailability.BeatSaberFavourites) ?? false;

        public bool IsFavouritesSelected
        {
            get => SongSource == SongSource.Favourites;
            set
            {
                if (value)
                {
                    SongSource = SongSource.Favourites;
                    OnPropertyChanged();
                }
            }
        }

        public bool SearchAvailable => userConfig?.PlaylistsConfig.SourceAvailability.HasFlag(PlaylistsSourceAvailability.SongSearch) ?? false;

        public bool IsSearchSelected
        {
            get => SongSource == SongSource.Search;
            set
            {
                if (value)
                {
                    SongSource = SongSource.Search;
                    OnPropertyChanged();
                }
            }
        }

        public bool SongSuggestAvailable => userConfig?.PlaylistsConfig.SourceAvailability.HasFlag(PlaylistsSourceAvailability.SongSuggest) ?? false;

        public bool IsSongSuggestSelected
        {
            get => SongSource == SongSource.SongSuggest;
            set
            {
                if (value)
                {
                    SongSource = SongSource.SongSuggest;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        public SongSourcesControlViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
            userConfig = serviceLocator.GetService<IUserConfigDomain>().Config;
        }

        public async Task LoadAsync(bool refresh)
        {

        }
    }
}
