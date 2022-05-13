using CSM.DataAccess.Entities.Offline;
using CSM.Framework.Extensions;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using CSM.Framework.Converter;

namespace CSM.UiLogic.Workspaces.Playlists
{
    public class PlaylistViewModel : BasePlaylistViewModel
    {
        private Playlist playlist;

        public ObservableCollection<PlaylistSongViewModel> Songs { get; }

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

        public ImageSource CoverImage
        {
            get
            {
                if (string.IsNullOrWhiteSpace(playlist.Image)) return null;
                return ImageConverter.BitmapFromBase64(playlist.Image.Split(',').Last());
            }
        }

        public string PlaylistAuthor
        {
            get => playlist.PlaylistAuthor;
            set
            {
                if (value != playlist.PlaylistAuthor) return;
                playlist.PlaylistAuthor = value;
                OnPropertyChanged();
            }
        }

        public string PlaylistDescription
        {
            get => playlist.PlaylistDescription;
            set
            {
                if (value != playlist.PlaylistDescription) return;
                PlaylistDescription = value;
                OnPropertyChanged();
            }
        }

        public PlaylistViewModel(Playlist playlist) : base(playlist.PlaylistTitle)
        {
            this.playlist = playlist;
            Songs = new ObservableCollection<PlaylistSongViewModel>();
            Songs.AddRange(playlist.Songs.Select(s => new PlaylistSongViewModel(s)));
        }
    }
}