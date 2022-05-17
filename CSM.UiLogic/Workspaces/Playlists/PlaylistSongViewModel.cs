using CSM.DataAccess.Entities.Offline;
using CSM.Framework.Extensions;
using CSM.Services;
using CSM.UiLogic.Properties;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
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
        private Playlist playlist;
        private PlaylistSong playlistSong;

        #region Public Properties

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

        #endregion

        /// <summary>
        /// Initializes a new <see cref="PlaylistSongViewModel"/>.
        /// </summary>
        /// <param name="playlistSong">The <see cref="PlaylistSong"/>.</param>
        public PlaylistSongViewModel(PlaylistSong playlistSong, Playlist playlist)
        {
            this.playlistSong = playlistSong;
            this.playlist = playlist;
            SetAvailableDifficultiesCommand = new AsyncRelayCommand(SetAvailableDifficultiesAsync);
            AvailableDifficulties = new ObservableCollection<PlaylistSongDifficultyViewModel>();

            Difficulties = new ObservableCollection<PlaylistSongDifficultyViewModel>();
            if (playlistSong.Difficulties != null)
            {
                Difficulties.AddRange(playlistSong.Difficulties.Select(d => new PlaylistSongDifficultyViewModel(d, true)));
            }
        }

        #region Helper methods

        private async Task SetAvailableDifficultiesAsync()
        {
            foreach (var difficulty in AvailableDifficulties)
            {
                difficulty.DifficultyChanged -= DifficultyViewModel_DifficultyChanged;
            }
            AvailableDifficulties.Clear();

            var beatmapService = new BeatMapService("maps/hash");
            var beatmap = await beatmapService.GetBeatMapDataAsync(playlistSong.Hash);

            var difficulties = beatmap.Versions.SelectMany(v => v.Difficulties);
            foreach (var difficulty in difficulties)
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