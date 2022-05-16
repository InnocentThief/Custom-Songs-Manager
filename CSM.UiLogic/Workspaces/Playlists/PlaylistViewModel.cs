using CSM.DataAccess.Entities.Offline;
using CSM.Framework.Converter;
using CSM.Framework.Extensions;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace CSM.UiLogic.Workspaces.Playlists
{
    /// <summary>
    /// Represents one playlist.
    /// </summary>
    public class PlaylistViewModel : BasePlaylistViewModel
    {
        #region Private fields

        private Playlist playlist;
        private PlaylistSongViewModel playlistSong;

        #endregion

        #region Public Properties

        /// <summary>
        /// Contains all songs of a playlist.
        /// </summary>
        public ObservableCollection<PlaylistSongViewModel> Songs { get; }

        /// <summary>
        /// Gets or sets the selected song.
        /// </summary>
        public PlaylistSongViewModel SelectedPlaylistSong
        {
            get => playlistSong;
            set
            {
                if (playlistSong == value) return;
                playlistSong = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the title of the playlist.
        /// </summary>
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

        /// <summary>
        /// Gets the cover image of the playlist.
        /// </summary>
        public ImageSource CoverImage
        {
            get
            {
                if (string.IsNullOrWhiteSpace(playlist.Image)) return null;
                return ImageConverter.BitmapFromBase64(playlist.Image.Split(',').Last());
            }
        }

        /// <summary>
        /// Gets or sets the author of the playlist.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the description of the playlist.
        /// </summary>
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

        #endregion

        /// <summary>
        /// Initializes a new <see cref="PlaylistViewModel"/>.
        /// </summary>
        /// <param name="playlist">The <see cref="Playlist"/>.</param>
        public PlaylistViewModel(Playlist playlist) : base(playlist.PlaylistTitle)
        {
            this.playlist = playlist;
            Songs = new ObservableCollection<PlaylistSongViewModel>();
            Songs.AddRange(playlist.Songs.Select(s => new PlaylistSongViewModel(s)));
        }
    }
}