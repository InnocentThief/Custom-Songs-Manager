using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;

namespace CSM.UiLogic.Workspaces.Playlists
{
    /// <summary>
    /// Base class for a playlist entry (can be a folder or a playlist).
    /// </summary>
    public abstract class BasePlaylistViewModel : ObservableObject
    {
        #region Private fields

        private bool containsSong;
        private string name;

        #endregion

        #region Public Properties

        /// <summary>
        /// Name of the playlist or folder.
        /// </summary>
        public string Name
        {
            get => name;
            set
            {
                if (value == name) return;
                name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets whether the playlist or the folder contains the song.
        /// </summary>
        public bool ContainsSong
        {
            get => containsSong;
            set
            {
                if (containsSong == value) return;
                containsSong = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the path of the playlist.
        /// </summary>
        /// <remarks>Could be file path or directory. Based on the type.</remarks>
        public string FilePath { get; }

        #endregion

        public event EventHandler<PlaylistSongChangedEventArgs> SongChangedEvent;

        /// <summary>
        /// Initializes a new <see cref="BasePlaylistViewModel"/>.
        /// </summary>
        /// <param name="name">The name of the playlist or folder.</param>
        public BasePlaylistViewModel(string name, string path)
        {
            Name = name;
            FilePath = path;
        }

        /// <summary>
        /// Checks if the playlist or folder contains the song with the given hash.
        /// </summary>
        /// <param name="hash">The hash of the song to check.</param>
        /// <returns>True if the playlist or folder contains the song.</returns>
        public abstract bool CheckContainsSong(string hash);

        /// <summary>
        /// Occurs when the selected playlist song changes.
        /// </summary>
        /// <param name="playlistSong"></param>
        protected void SongChanged(PlaylistSongViewModel playlistSong)
        {
            if (playlistSong == null) return;
            SongChangedEvent?.Invoke(this, new PlaylistSongChangedEventArgs() { Hash = playlistSong.Hash });
        }
    }
}