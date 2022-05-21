using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;

namespace CSM.UiLogic.Workspaces.Playlists
{
    /// <summary>
    /// Base class for a playlist entry (can be a folder or a playlist).
    /// </summary>
    public abstract class BasePlaylistViewModel : ObservableObject
    {
        private bool containsSong;

        /// <summary>
        /// Name of the playlist or folder.
        /// </summary>
        public string Name { get; set; }

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

        public abstract bool CheckContainsSong(string songName);

        protected void SongChanged(PlaylistSongViewModel playlistSong)
        {
            if (playlistSong == null) return;
            SongChangedEvent?.Invoke(this, new PlaylistSongChangedEventArgs() { Hash = playlistSong.Hash });
        }
    }
}