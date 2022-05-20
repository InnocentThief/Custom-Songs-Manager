using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace CSM.UiLogic.Workspaces.Playlists
{
    /// <summary>
    /// Represents a folder in the playlists list.
    /// </summary>
    public class PlaylistFolderViewModel : BasePlaylistViewModel
    {
        /// <summary>
        /// Gets the directory path of the folder.
        /// </summary>
        public string FolderPath { get; }

        /// <summary>
        /// Contains all playlist in the current folder (sub folders or playlists).
        /// </summary>
        public ObservableCollection<BasePlaylistViewModel> Playlists { get; set; }

        /// <summary>
        /// Initializes a new <see cref="PlaylistFolderViewModel"/>.
        /// </summary>
        /// <param name="path">The path to the directory.</param>
        public PlaylistFolderViewModel(string path) : base(Path.GetFileName(path))
        {
            Playlists = new ObservableCollection<BasePlaylistViewModel>();
            FolderPath = path;
        }

        public override bool CheckContainsSong(string hash)
        {
            ContainsSong = Playlists.Any(p => p.CheckContainsSong(hash));
            return ContainsSong;
        }
    }
}