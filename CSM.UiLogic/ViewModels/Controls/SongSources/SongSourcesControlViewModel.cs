using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Controls.SongSources
{
    internal class SongSourcesControlViewModel : BaseViewModel
    {
        private SongSource songSource = SongSource.CustomLevels;

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

        public SongSourcesControlViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {

        }

        public async Task LoadAsync(bool refresh)
        {

        }
    }
}
