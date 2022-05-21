using System;

namespace CSM.UiLogic.Workspaces.Playlists
{
    public class AddSongToPlaylistEventArgs : EventArgs
    {
        public string BsrKey { get; set; }

        public string Hash { get; set; }

        public string SongName { get; set; }

        public string LevelAuthorName { get; set; }

        public string LevelId { get; set; }
    }
}