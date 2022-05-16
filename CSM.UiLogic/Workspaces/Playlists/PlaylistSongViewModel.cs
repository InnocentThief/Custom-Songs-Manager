using CSM.DataAccess.Entities.Offline;
using System.Collections.Generic;
using System.Linq;

namespace CSM.UiLogic.Workspaces.Playlists
{
    /// <summary>
    /// Represents one song inside a playlist.
    /// </summary>
    public class PlaylistSongViewModel
    {
        private PlaylistSong playlistSong;

        #region Public Properties

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
        /// Gets a list of difficulties this playlist song contains.
        /// </summary>
        public List<PlaylistSongDifficultyViewModel> Difficulties { get; }

        /// <summary>
        /// Gets the the difficulties this playlist song contains as string.
        /// </summary>
        public string Difficulty
        {
            get
            {
                if (playlistSong.Difficulties == null) return string.Empty;
                var characteristics = playlistSong.Difficulties.GroupBy(d => d.Characteristic);
                return string.Join(" / ", characteristics.Select(c => $"{c.Key} ({string.Join(", ", c.Select(d => d.Name))})"));
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new <see cref="PlaylistSongViewModel"/>.
        /// </summary>
        /// <param name="playlistSong">The <see cref="PlaylistSong"/>.</param>
        public PlaylistSongViewModel(PlaylistSong playlistSong)
        {
            this.playlistSong = playlistSong;
            Difficulties = new List<PlaylistSongDifficultyViewModel>();
            if (playlistSong.Difficulties != null)
            {
                Difficulties.AddRange(playlistSong.Difficulties.Select(d => new PlaylistSongDifficultyViewModel(d)));
            }
        }
    }
}