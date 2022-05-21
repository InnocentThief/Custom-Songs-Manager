using System;

namespace CSM.UiLogic.Workspaces.Playlists
{
    public class PlaylistSongChangedEventArgs : EventArgs
    {
        public string Hash { get; set; }
    }
}