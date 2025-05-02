using CSM.DataAccess.Playlists;

namespace CSM.Business.Core.SongCopy
{
    internal class SongCopyEventArgs : EventArgs
    {
        public bool OverwritePlaylist { get; set; }

        public List<Song> Songs { get; set; } = [];
    }
}
