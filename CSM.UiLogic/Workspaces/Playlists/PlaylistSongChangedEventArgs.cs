using System;

namespace CSM.UiLogic.Workspaces.Playlists
{
    /// <summary>
    /// Event Args used when a song selection on a playlist changes.
    /// </summary>
    public class PlaylistSongChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Hash of the selected song.
        /// </summary>
        public string Hash { get; set; }
    }
}