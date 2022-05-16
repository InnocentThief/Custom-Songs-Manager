using CSM.DataAccess.Entities.Offline;

namespace CSM.UiLogic.Workspaces.Playlists
{
    /// <summary>
    /// Represents a difficulty for a playlist song.
    /// </summary>
    public class PlaylistSongDifficultyViewModel
    {
        /// <summary>
        /// Gets the song difficulty data.
        /// </summary>

        private PlaylistSongDifficulty playlistSongDifficulty;

        /// <summary>
        /// Gets the difficulty characteristics.
        /// </summary>
        public string Characteristic => playlistSongDifficulty.Characteristic;

        /// <summary>
        /// Gets the name of the difficulty.
        /// </summary>
        public string Name => playlistSongDifficulty.Name;

        /// <summary>
        /// Initializes a new <see cref="PlaylistSongDifficultyViewModel"/>.
        /// </summary>
        /// <param name="playlistSongDifficulty">The <see cref="PlaylistSongDifficulty"/>.</param>
        public PlaylistSongDifficultyViewModel(PlaylistSongDifficulty playlistSongDifficulty)
        {
            this.playlistSongDifficulty = playlistSongDifficulty;
        }
    }
}