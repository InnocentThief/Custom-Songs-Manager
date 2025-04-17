using CSM.Business.Interfaces;
using CSM.DataAccess.Playlists;
using CSM.Framework.Extensions;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.Commands;
using CSM.UiLogic.Converter;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace CSM.UiLogic.ViewModels.Common.Playlists
{
    internal sealed class PlaylistViewModel : BasePlaylistViewModel
    {
        #region Private fields

        private readonly Playlist playlist;
        private IRelayCommand? fetchDataCommand;

        private readonly IBeatSaverService beatSaverService;

        #endregion

        #region Properties

        public IRelayCommand? FetchDataCommand => fetchDataCommand ??= CommandFactory.CreateFromAsync(FetchDataAsync, CanFetchData);

        public string PlaylistTitle
        {
            get => playlist.PlaylistTitle;
            set
            {
                if (value == playlist.PlaylistTitle) return;
                playlist.PlaylistTitle = value;
                OnPropertyChanged();
            }
        }

        public string PlaylistAuthor
        {
            get => playlist.PlaylistAuthor;
            set
            {
                if (value == playlist.PlaylistAuthor) return;
                playlist.PlaylistAuthor = value;
                OnPropertyChanged();
            }
        }

        public string? PlaylistDescription
        {
            get => playlist.PlaylistDescription;
            set
            {
                if (value == playlist.PlaylistDescription) return;
                playlist.PlaylistDescription = value;
                OnPropertyChanged();
            }
        }

        public ImageSource? CoverImage
        {
            get
            {
                if (string.IsNullOrWhiteSpace(playlist.Image)) return null;
                return ImageConverter.BitmapFromBase64(playlist.Image.Split(',').Last());
            }
        }

        public ObservableCollection<PlaylistSongViewModel> Songs { get; } = [];

        #endregion

        public PlaylistViewModel(
           IServiceLocator serviceLocator,
           Playlist playlist,
           string path) : base(serviceLocator, playlist.PlaylistTitle, path)
        {
            this.playlist = playlist;
            beatSaverService = serviceLocator.GetService<IBeatSaverService>();

            Songs.AddRange(playlist.Songs.Select(s => new PlaylistSongViewModel(serviceLocator, s)));
        }

        public async Task FetchDataAsync()
        {

        }

        #region Helper methods

        private bool CanFetchData()
        {
            return true;
        }

        #endregion

    }
}
