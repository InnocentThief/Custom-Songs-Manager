using CSM.DataAccess.Playlists;

namespace CSM.Business.Core.SongCopy
{
    internal class SongCopyEventArgs : EventArgs
    {
        public List<Song> Songs { get; set; } = [];
    }
}
