using CSM.Business.Core.SongCopy;
using CSM.Business.Interfaces.SongCopy;

namespace CSM.Business.Interfaces
{
    internal interface ISongCopyDomain
    {
        IBasePlaylistViewModel? SelectedPlaylist { get; }

        event EventHandler<SongCopyEventArgs>? OnCopySongs;

        event EventHandler<CreatePlaylistEventArgs>? OnCreatePlaylist;

        event EventHandler<PlaylistSelectionChangedEventArgs>? OnPlaylistSelectionChanged;

        void CopySongs(SongCopyEventArgs songCopyEventArgs);

        void CreatePlaylist(CreatePlaylistEventArgs createPlaylistEventArgs);

        void SetSelectedPlaylist(IBasePlaylistViewModel? playlist);
    }
}
