using CSM.DataAccess.Entities.Offline;

namespace CSM.UiLogic.Workspaces.Playlists
{
    public class PlaylistViewModel : BasePlaylistViewModel
    {
        private Playlist playlist;

        public PlaylistViewModel(Playlist playlist) : base(playlist.PlaylistTitle)
        {
            this.playlist = playlist;
        }
    }
}