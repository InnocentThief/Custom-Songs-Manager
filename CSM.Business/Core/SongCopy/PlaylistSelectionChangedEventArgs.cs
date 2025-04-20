using CSM.Business.Interfaces.SongCopy;

namespace CSM.Business.Core.SongCopy
{
    internal class PlaylistSelectionChangedEventArgs : EventArgs
    {
            public IBasePlaylistViewModel? Playlist { get; set; }
    }
}
