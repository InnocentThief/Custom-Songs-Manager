using CSM.DataAccess.Entities.Offline;

namespace CSM.UiLogic.Workspaces.Playlists
{
    /// <summary>
    /// Represents one song inside a playlist.
    /// </summary>
    public class PlaylistSongViewModel
    {
        private PlaylistSong playlistSong;

        /// <summary>
        /// The name of the song.
        /// </summary>
        /// <remarks>Can be string empty as it is not mandatory to provide this information.</remarks>
        public string SongName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(playlistSong.SongName)) return playlistSong.SongName;
                return "NA (Try to use Tools to fix this issue)";
            }
        }

        /// <summary>
        /// Initializes a new <see cref="PlaylistSongViewModel"/>.
        /// </summary>
        /// <param name="playlistSong">The <see cref="PlaylistSong"/>.</param>
        public PlaylistSongViewModel(PlaylistSong playlistSong)
        {
            this.playlistSong = playlistSong;
        }
    }
}