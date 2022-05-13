using System.Collections.Generic;
using System.IO;

namespace CSM.UiLogic.Workspaces.Playlists
{
    public class PlaylistFolderViewModel : BasePlaylistViewModel
    {
        public string FolderPath { get; }

        public List<BasePlaylistViewModel> Playlists { get; set; }

        public PlaylistFolderViewModel(string path) : base(Path.GetFileName(path))
        {
            Playlists = new List<BasePlaylistViewModel>();
            FolderPath = path;
        }
    }
}