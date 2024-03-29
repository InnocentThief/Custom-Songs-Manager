﻿using CSM.DataAccess.Entities.Offline;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Text;

namespace CSM.UiLogic.Workspaces.Playlists
{
    /// <summary>
    /// Represents a difficulty for a playlist song.
    /// </summary>
    public class PlaylistSongDifficultyViewModel : ObservableObject
    {
        #region Private fields

        private bool isSelectedDifficulty;
        private readonly PlaylistSongDifficulty playlistSongDifficulty;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the difficulty characteristics.
        /// </summary>
        public string Characteristic => playlistSongDifficulty.Characteristic;

        /// <summary>
        /// Gets the name of the difficulty.
        /// </summary>
        public string Name => playlistSongDifficulty.Name;

        /// <summary>
        /// Gets the characteristic combined with the difficulty.
        /// </summary>
        public string Difficulty => $"{Characteristic} {Name}";

        /// <summary>
        /// Gets the difficulty detail (NPS, Chroma, Mapping Extensions, Noodle).
        /// </summary>
        public string DifficultyDetail
        {
            get
            {
                var difficultyDetail = new StringBuilder();
                difficultyDetail.Append($"NPS: {playlistSongDifficulty.NPS}");
                if (playlistSongDifficulty.Chroma) difficultyDetail.Append(" / cr");
                if (playlistSongDifficulty.MappingExtensions) difficultyDetail.Append(" / me");
                if (playlistSongDifficulty.Noodle) difficultyDetail.Append(" / no");
                return difficultyDetail.ToString();
            }
        }

        /// <summary>
        /// Gets the start rating of a difficulty.
        /// </summary>
        /// <remarks>The difficulty string if not ranked; otherwise the ranking.</remarks>
        public string Stars
        {
            get
            {
                return playlistSongDifficulty.Stars > 0 ? $"{playlistSongDifficulty.Stars}*" : String.Empty;
            }
        }

        public string Label
        {
            get
            {
                return playlistSongDifficulty.Label;
            }
        }

        public string DisplayText
        {
            get
            {
                if (playlistSongDifficulty.Stars > 0 && !string.IsNullOrEmpty(playlistSongDifficulty.Label)) return $"{playlistSongDifficulty.Stars}* {playlistSongDifficulty.Label}";
                if (playlistSongDifficulty.Stars > 0) return $"{playlistSongDifficulty.Stars}* {Name}";
                if (!string.IsNullOrWhiteSpace(playlistSongDifficulty.Label)) return playlistSongDifficulty.Label;
                return Name;
            }
        }

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

        #endregion

        /// <summary>
        /// Occures when the difficulty changes.
        /// </summary>
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