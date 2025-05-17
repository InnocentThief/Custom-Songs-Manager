using CSM.Business.Core.SongSelection;
using CSM.Business.Interfaces;
using CSM.DataAccess.UserConfiguration;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.ViewModels.Controls.BeatLeader;
using CSM.UiLogic.ViewModels.Controls.CustomLevels;

namespace CSM.UiLogic.ViewModels.Controls.SongSources
{
    internal class SongSourcesControlViewModel : BaseViewModel
    {
        #region Private fields

        private ISongSourceViewModel? selectedSource;

        private readonly UserConfig? userConfig;
        private readonly ISongSelectionDomain songSelectionDomain;

        #endregion

        #region Properties

        public bool AnySourcesAvailable => CustomLevelsAvailable || PlaylistsAvailable || FavouritesAvailable || SearchAvailable || SongSuggestAvailable || BeatLeaderAvailable;

        public List<ISongSourceViewModel> Sources { get; set; } = [];

        public ISongSourceViewModel? SelectedSource
        {
            get => selectedSource;
            set
            {
                if (value == selectedSource)
                    return;
                selectedSource = value;
                OnPropertyChanged();
            }
        }

        public bool CustomLevelsAvailable => userConfig?.PlaylistsConfig.SourceAvailability.HasFlag(PlaylistsSourceAvailability.CustomLevels) ?? false;

        public bool IsCustomLevelsSelected
        {
            get => selectedSource is CustomLevelsControlViewModel;
            set
            {
                if (value)
                {
                    SelectedSource = Sources.SingleOrDefault(s => s is CustomLevelsControlViewModel);
                    OnPropertyChanged();
                    songSelectionDomain.SetSongHash(null, SongSelectionType.Right);
                }
            }
        }

        public bool PlaylistsAvailable => userConfig?.PlaylistsConfig.SourceAvailability.HasFlag(PlaylistsSourceAvailability.Playlists) ?? false;

        public bool IsPlaylistsSelected
        {
            get => selectedSource is PlaylistsSourceViewModel;
            set
            {
                if (value)
                {
                    SelectedSource = Sources.SingleOrDefault(s => s is PlaylistsSourceViewModel);
                    OnPropertyChanged();
                    songSelectionDomain.SetSongHash(null, SongSelectionType.Right);
                }
            }
        }

        public bool FavouritesAvailable => userConfig?.PlaylistsConfig.SourceAvailability.HasFlag(PlaylistsSourceAvailability.BeatSaberFavourites) ?? false;

        public bool IsFavouritesSelected
        {
            get => selectedSource is BeatSaberFavouritesSourceViewModel;
            set
            {
                if (value)
                {
                    SelectedSource = Sources.SingleOrDefault(s => s is BeatSaberFavouritesSourceViewModel);
                    OnPropertyChanged();
                    songSelectionDomain.SetSongHash(null, SongSelectionType.Right);
                }
            }
        }

        public bool SearchAvailable => userConfig?.PlaylistsConfig.SourceAvailability.HasFlag(PlaylistsSourceAvailability.SongSearch) ?? false;

        public bool IsSearchSelected
        {
            get => selectedSource is SongSearchSourceViewModel;
            set
            {
                if (value)
                {
                    SelectedSource = Sources.SingleOrDefault(s => s is SongSearchSourceViewModel);
                    OnPropertyChanged();
                    songSelectionDomain.SetSongHash(null, SongSelectionType.Right);
                }
            }
        }

        public bool SongSuggestAvailable => userConfig?.PlaylistsConfig.SourceAvailability.HasFlag(PlaylistsSourceAvailability.SongSuggest) ?? false;

        public bool IsSongSuggestSelected
        {
            get => selectedSource is SongSuggestSourceViewModel;
            set
            {
                if (value)
                {
                    SelectedSource = Sources.SingleOrDefault(s => s is SongSuggestSourceViewModel);
                    OnPropertyChanged();
                    songSelectionDomain.SetSongHash(null, SongSelectionType.Right);
                }
            }
        }

        public bool TwitchAvailable => userConfig?.PlaylistsConfig.SourceAvailability.HasFlag(PlaylistsSourceAvailability.Twitch) ?? false;

        public bool IsTwitchSelected
        {
            get => selectedSource is TwitchSourceViewModel;
            set
            {
                if (value)
                {
                    SelectedSource = Sources.SingleOrDefault(s => s is TwitchSourceViewModel);
                    OnPropertyChanged();
                    songSelectionDomain.SetSongHash(null, SongSelectionType.Right);
                }
            }
        }

        public bool BeatLeaderAvailable => userConfig?.PlaylistsConfig.SourceAvailability.HasFlag(PlaylistsSourceAvailability.BeatLeader) ?? false;

        public bool IsBeatLeaderSelected
        {
            get => selectedSource is BeatLeaderControlViewModel;
            set
            {
                if (value)
                {
                    SelectedSource = Sources.SingleOrDefault(s => s is BeatLeaderControlViewModel);
                    OnPropertyChanged();
                    songSelectionDomain.SetSongHash(null, SongSelectionType.Right);
                }
            }
        }

        #endregion

        public SongSourcesControlViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
            userConfig = serviceLocator.GetService<IUserConfigDomain>().Config;
            songSelectionDomain = serviceLocator.GetService<ISongSelectionDomain>();

            if (CustomLevelsAvailable)
            {
                Sources.Add(new CustomLevelsControlViewModel(serviceLocator));
            }
            if (PlaylistsAvailable)
            {
                Sources.Add(new PlaylistsSourceViewModel(serviceLocator));
            }
            if (FavouritesAvailable)
            {
                Sources.Add(new BeatSaberFavouritesSourceViewModel(serviceLocator));
            }
            if (SearchAvailable)
            {
                Sources.Add(new SongSearchSourceViewModel(serviceLocator));
            }
            if (SongSuggestAvailable)
            {
                Sources.Add(new SongSuggestSourceViewModel(serviceLocator));
            }
            if (TwitchAvailable)
            {
                Sources.Add(new TwitchSourceViewModel(serviceLocator));
            }
            if (BeatLeaderAvailable)
            {
                Sources.Add(new BeatLeaderControlViewModel(serviceLocator));
            }

            switch (userConfig?.PlaylistsConfig.DefaultSource)
            {
                case PlaylistsSourceAvailability.CustomLevels:
                    IsCustomLevelsSelected = true;
                    break;
                case PlaylistsSourceAvailability.Playlists:
                    IsPlaylistsSelected = true;
                    break;
                case PlaylistsSourceAvailability.BeatSaberFavourites:
                    IsFavouritesSelected = true;
                    break;
                case PlaylistsSourceAvailability.SongSearch:
                    IsSearchSelected = true;
                    break;
                case PlaylistsSourceAvailability.SongSuggest:
                    IsSongSuggestSelected = true;
                    break;
                case PlaylistsSourceAvailability.Twitch:
                    IsTwitchSelected = true;
                    break;
                case PlaylistsSourceAvailability.BeatLeader:
                    IsBeatLeaderSelected = true;
                    break;
                default:
                    break;
            }
        }

        public async Task LoadAsync()
        {
            if (selectedSource is CustomLevelsControlViewModel customLevelsControlViewModel)
            {
                await customLevelsControlViewModel.LoadAsync(false);
            }

            if (selectedSource is PlaylistsSourceViewModel playlistsSourceViewModel)
            {
                await playlistsSourceViewModel.LoadAsync();
            }

            if (selectedSource is BeatSaberFavouritesSourceViewModel favouritesSourceViewModel)
            {
                await favouritesSourceViewModel.LoadAsync();
            }

            if (selectedSource is SongSearchSourceViewModel songSearchSourceViewModel)
            {
                // nothing to load
            }

            if (selectedSource is SongSuggestSourceViewModel songSuggestSourceViewModel)
            {
                await songSuggestSourceViewModel.LoadAsync();
            }

            if (selectedSource is TwitchSourceViewModel twitchSourceViewModel)
            {
                await twitchSourceViewModel.LoadAsync();
            }

            if (selectedSource is BeatLeaderControlViewModel beatLeaderControlViewModel)
            {
                await beatLeaderControlViewModel.LoadAsync(false);
            }
        }
    }
}
