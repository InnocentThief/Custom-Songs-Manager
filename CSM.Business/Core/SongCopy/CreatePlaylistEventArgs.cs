using CSM.DataAccess.Playlists;

namespace CSM.Business.Core.SongCopy
{
    internal class CreatePlaylistEventArgs
    {
        public string PlaylistName { get; set; } = string.Empty;

        public List<Song> Songs { get; set; } = [];
    }
}
