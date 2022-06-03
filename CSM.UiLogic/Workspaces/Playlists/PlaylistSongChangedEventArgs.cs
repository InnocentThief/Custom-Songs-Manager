using System;

namespace CSM.UiLogic.Workspaces.Playlists
{
    /// <summary>
    /// Event Args used when a song selection on a playlist changes.
    /// </summary>
    public class PlaylistSongChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Hash of the selected playlist song.
        /// </summary>
        public string LeftHash { get; set; }

        /// <summary>
        /// Hash of the selected custom level, beat saber favorite, or searched song.
        /// </summary>
        public string RightHash { get; set; }
    }
}