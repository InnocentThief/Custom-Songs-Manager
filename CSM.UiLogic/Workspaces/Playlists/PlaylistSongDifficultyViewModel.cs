using CSM.DataAccess.Entities.Offline;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;

namespace CSM.UiLogic.Workspaces.Playlists
{
    /// <summary>
    /// Represents a difficulty for a playlist song.
    /// </summary>
    public class PlaylistSongDifficultyViewModel : ObservableObject
    {
        private bool isSelectedDifficulty;

        /// <summary>
        /// Gets the song difficulty data.
        /// </summary>

        private PlaylistSongDifficulty playlistSongDifficulty;

        /// <summary>
        /// Gets the difficulty characteristics.
        /// </summary>
        public string Characteristic => playlistSongDifficulty.Characteristic;

        /// <summary>
        /// Gets the name of the difficulty.
        /// </summary>
        public string Name => playlistSongDifficulty.Name;

        public string Difficulty => $"{Characteristic} {Name}";

        /// <summary>
        /// Gets or sets whether the current difficulty is part of the song in the playlist.
        /// </summary>
        public bool IsSelectedDifficulty
        {
            get => isSelectedDifficulty;
            set
            {
                if (value == isSelectedDifficulty) return;
                isSelectedDifficulty = value;
                OnPropertyChanged();
                DifficultyChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler DifficultyChanged;

        /// <summary>
        /// Initializes a new <see cref="PlaylistSongDifficultyViewModel"/>.
        /// </summary>
        /// <param name="playlistSongDifficulty">The <see cref="PlaylistSongDifficulty"/>.</param>
        public PlaylistSongDifficultyViewModel(PlaylistSongDifficulty playlistSongDifficulty, bool isSelectedDifficulty)
        {
            this.playlistSongDifficulty = playlistSongDifficulty;
            IsSelectedDifficulty = isSelectedDifficulty;
        }
    }
}