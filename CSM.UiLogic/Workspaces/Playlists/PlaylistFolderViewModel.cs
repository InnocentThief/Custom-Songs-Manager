﻿using System.Collections.ObjectModel;
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
        /// Contains all playlist in the current folder (sub folders or playlists).
        /// </summary>
        public ObservableCollection<BasePlaylistViewModel> Playlists { get; set; }

        /// <summary>
        /// Initializes a new <see cref="PlaylistFolderViewModel"/>.
        /// </summary>
        /// <param name="path">The path to the directory.</param>
        public PlaylistFolderViewModel(string path) : base(Path.GetFileName(path), path)
        {
            Playlists = new ObservableCollection<BasePlaylistViewModel>();
        }

        /// <summary>
        /// Checks if the folder contains the song with the given hash (Check for playlist songs).
        /// </summary>
        /// <param name="leftHash">The hash of the song to check.</param>
        /// <returns>True if the folder contains the song.</returns>
        public override bool CheckContainsLeftSong(string leftHash)
        {
            ContainsLeftSong = Playlists.Any(p => p.CheckContainsLeftSong(leftHash));
            return ContainsLeftSong;
        }

        /// <summary>
        /// Checks if the folder contains the song with the given hash (Check for custom songs, favorites, and song search).
        /// </summary>
        /// <param name="rightHash">The hash of the song to check.</param>
        /// <returns>True if the folder contains the song.</returns>
        public override bool CheckContainsRightSong(string rightHash)
        {
            ContainsRightSong = Playlists.Any(p => p.CheckContainsRightSong(rightHash));
            return ContainsRightSong;
        }
    }
}