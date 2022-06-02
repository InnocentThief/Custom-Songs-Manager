using CSM.DataAccess.Entities.Offline;
using CSM.Framework.Extensions;
using CSM.Services;
using CSM.UiLogic.Properties;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.Playlists
{
    /// <summary>
    /// Represents one song inside a playlist.
    /// </summary>
    public class PlaylistSongViewModel : ObservableObject
    {
        #region Private fields

        private Playlist playlist;
        private PlaylistSong playlistSong;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the hash of the song.
        /// </summary>
        public string Hash
        {
            get => playlistSong.Hash.ToLower();
        }

        /// <summary>
        /// Gets the bsr key of the song.
        /// </summary>
        public string BsrKey => playlistSong.Key;

        public int BsrKeyHex
        {
            get
            {
                if (string.IsNullOrWhiteSpace(playlistSong.Key)) return 0;
                return int.Parse(playlistSong.Key, System.Globalization.NumberStyles.HexNumber);
            }
        }

        /// <summary>
        /// The name of the song.
        /// </summary>
        /// <remarks>Can be string empty as it is not mandatory to provide this information.</remarks>
        public string SongName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(playlistSong.SongName)) return playlistSong.SongName;
                return Resources.Playlists_SongName_NA;
            }
        }

        /// <summary>
        /// Gets the name of the author.
        /// </summary>
        public string LevelAuthorName
        {
            get => playlistSong.LevelAuthorName;
        }

        /// <summary>
        /// Gets a list of difficulties this playlist song contains.
        /// </summary>
        public ObservableCollection<PlaylistSongDifficultyViewModel> Difficulties { get; }

        /// <summary>
        /// Gets the the difficulties this playlist song contains as string.
        /// </summary>
        public string Difficulty
        {
            get
            {
                if (playlistSong.Difficulties == null) return string.Empty;
                var characteristics = playlistSong.Difficulties.GroupBy(d => d.Characteristic);
                return string.Join(" / ", characteristics.Select(c => $"{c.Key} ({string.Join(", ", c.Select(d => d.Name))})"));
            }
        }

        public ObservableCollection<PlaylistSongDifficultyViewModel> AvailableDifficulties { get; }

        public AsyncRelayCommand SetAvailableDifficultiesCommand { get; }

        public RelayCommand DeleteSongCommand { get; }

        #endregion

        /// <summary>
        /// Occurs on song deletion.
        /// </summary>
        public event EventHandler DeleteSongEvent;

        /// <summary>
        /// Initializes a new <see cref="PlaylistSongViewModel"/>.
        /// </summary>
        /// <param name="playlistSong">The <see cref="PlaylistSong"/>.</param>
        public PlaylistSongViewModel(PlaylistSong playlistSong, Playlist playlist)
        {
            this.playlistSong = playlistSong;
            this.playlist = playlist;
            SetAvailableDifficultiesCommand = new AsyncRelayCommand(SetAvailableDifficultiesAsync);
            DeleteSongCommand = new RelayCommand(DeleteSong);
            AvailableDifficulties = new ObservableCollection<PlaylistSongDifficultyViewModel>();

            Difficulties = new ObservableCollection<PlaylistSongDifficultyViewModel>();
            if (playlistSong.Difficulties != null)
            {
                Difficulties.AddRange(playlistSong.Difficulties.Select(d => new PlaylistSongDifficultyViewModel(d, true)));
            }
        }

        #region Helper methods

        private void DeleteSong()
        {
            playlist.Songs.Remove(playlistSong);
            DeleteSongEvent?.Invoke(this, EventArgs.Empty);
        }

        private async Task SetAvailableDifficultiesAsync()
        {
            foreach (var difficulty in AvailableDifficulties)
            {
                difficulty.DifficultyChanged -= DifficultyViewModel_DifficultyChanged;
            }
            AvailableDifficulties.Clear();

            var beatmapService = new BeatMapService("maps/hash");
            var beatmap = await beatmapService.GetBeatMapDataAsync(playlistSong.Hash);

            foreach (var difficulty in beatmap.LatestVersion.Difficulties)
            {
                var difficultyViewModel = Difficulties.SingleOrDefault(d => d.Characteristic == difficulty.Characteristic && d.Name == difficulty.Diff);
                if (difficultyViewModel == null)
                {
                    var playlistSongDifficulty = new PlaylistSongDifficulty
                    {
                        Characteristic = difficulty.Characteristic,
                        Name = difficulty.Diff
                    };
                    difficultyViewModel = new PlaylistSongDifficultyViewModel(playlistSongDifficulty, false);
                }
                difficultyViewModel.DifficultyChanged += DifficultyViewModel_DifficultyChanged;
                AvailableDifficulties.Add(difficultyViewModel);
            }
        }

        private void DifficultyViewModel_DifficultyChanged(object sender, System.EventArgs e)
        {
            var viewModel = sender as PlaylistSongDifficultyViewModel;
            if (viewModel.IsSelectedDifficulty)
            {
                var difficulty = new PlaylistSongDifficulty
                {
                    Characteristic = viewModel.Characteristic,
                    Name = viewModel.Name,
                };
                if (playlistSong.Difficulties == null) playlistSong.Difficulties = new List<PlaylistSongDifficulty>();
                playlistSong.Difficulties.Add(difficulty);
                Difficulties.Add(viewModel);
            }
            else
            {
                var existingViewModel = Difficulties.SingleOrDefault(d => d.Characteristic == viewModel.Characteristic && d.Name == viewModel.Name);
                existingViewModel.DifficultyChanged -= DifficultyViewModel_DifficultyChanged;
                if (existingViewModel != null) Difficulties.Remove(existingViewModel);
                var existingEntity = playlistSong.Difficulties.SingleOrDefault(d => d.Characteristic == viewModel.Characteristic && d.Name == viewModel.Name);
                if (existingEntity != null) playlistSong.Difficulties.Remove(existingEntity);
            }

            // Save to file
            var options = new JsonSerializerOptions { WriteIndented = true };
            var content = JsonSerializer.Serialize(playlist, options);
            File.WriteAllText(playlist.Path, content);
        }

        #endregion
    }
}