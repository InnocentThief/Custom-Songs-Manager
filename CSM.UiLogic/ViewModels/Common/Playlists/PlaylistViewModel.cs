using CSM.DataAccess.Playlists;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.Converter;
using System.Windows.Media;

namespace CSM.UiLogic.ViewModels.Common.Playlists
{
    internal sealed class PlaylistViewModel(
        IServiceLocator serviceLocator, 
        Playlist playlist,
        string path) 
        : BasePlaylistViewModel(serviceLocator, playlist.PlaylistTitle, path)
    {
        private readonly Playlist playlist = playlist;

        #region Properties

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

        #endregion

    }
}
