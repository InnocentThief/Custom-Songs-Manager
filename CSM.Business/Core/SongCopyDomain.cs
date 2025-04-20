using CSM.Business.Core.SongCopy;
using CSM.Business.Interfaces;
using CSM.Business.Interfaces.SongCopy;

namespace CSM.Business.Core
{
    internal class SongCopyDomain : ISongCopyDomain
    {
        private IBasePlaylistViewModel? selectedPlaylist;

        public IBasePlaylistViewModel? SelectedPlaylist
        {
            get => selectedPlaylist;
            private set
            {
                if (value == selectedPlaylist) return;
                selectedPlaylist = value;
            }
        }

        public event EventHandler<PlaylistSelectionChangedEventArgs>? OnPlaylistSelectionChanged;
        public event EventHandler<SongCopyEventArgs>? OnCopySongs;
        public event EventHandler<CreatePlaylistEventArgs>? OnCreatePlaylist;

        public void CopySongs(SongCopyEventArgs songCopyEventArgs)
        {
            OnCopySongs?.Invoke(this, songCopyEventArgs);
        }

        public void CreatePlaylist(CreatePlaylistEventArgs createPlaylistEventArgs)
        {
            OnCreatePlaylist?.Invoke(this, createPlaylistEventArgs);
        }

        public void SetSelectedPlaylist(IBasePlaylistViewModel? playlist)
        {
            SelectedPlaylist = playlist;
            OnPlaylistSelectionChanged?.Invoke(this, new PlaylistSelectionChangedEventArgs
            {
                Playlist = playlist
            });
        }
    }
}
