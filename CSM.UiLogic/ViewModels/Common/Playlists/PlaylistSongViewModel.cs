using CSM.Business.Core.SongCopy;
using CSM.Business.Interfaces;
using CSM.DataAccess.BeatSaver;
using CSM.DataAccess.Playlists;
using CSM.Framework.Extensions;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;
using CSM.UiLogic.ViewModels.Common.MapDetails;
using System.Collections.ObjectModel;
using System.Globalization;

namespace CSM.UiLogic.ViewModels.Common.Playlists
{
    internal class PlaylistSongViewModel : BaseViewModel
    {
        #region Private fields

        private MapDetailViewModel? mapDetailViewModel;
        private IRelayCommand? addToPlaylistCommand, removeFromPlaylistCommand, setAvailableDifficultiesCommand;

        private readonly Song song;
        private readonly List<PlaylistSongDifficultyViewModel> difficulties = [];
        private readonly IBeatSaverService beatSaverService;
        private readonly ISongCopyDomain songCopyDomain;

        #endregion

        #region Properties

        public Song Model => song;

        public string Hash => song.Hash;

        public string BsrKey => song.Key ?? string.Empty;

        public int BsrKeyHex
        {
            get
            {
                if (string.IsNullOrWhiteSpace(song.Key))
                    return 0;
                int.TryParse(BsrKey, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int result);
                return result;
            }
        }

        public string SongName => song.SongName;

        public string? LevelAuthorName => song.LevelAuthorName;

        public ObservableCollection<PlaylistSongDifficultyViewModel> AvailableDifficulties { get; } = [];

        public List<PlaylistSongDifficultyViewModel> Difficulties => [.. difficulties.OrderBy(d => d.Characteristic).ThenBy(d => d.Difficulty)];

        public DateTime? Uploaded { get; private set; }

        public MapDetailViewModel? MapDetailViewModel
        {
            get => mapDetailViewModel;
            set
            {
                if (mapDetailViewModel == value)
                    return;
                mapDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        public IRelayCommand? AddToPlaylistCommand => addToPlaylistCommand ??= CommandFactory.CreateFromAsync(AddToPlaylistAsync, CanAddToPlaylist);
        public IRelayCommand? RemoveFromPlaylistCommand => removeFromPlaylistCommand ??= CommandFactory.Create(RemoveFromPlaylist, CanRemoveFromPlaylist);
        public IRelayCommand? SetAvailableDifficultiesCommand => setAvailableDifficultiesCommand ??= CommandFactory.CreateFromAsync(SetAvailableDifficultiesAsync, CanSetAvailableDifficulties);

        #endregion

        public event EventHandler? OnSongRemoved;

        public PlaylistSongViewModel(IServiceLocator serviceLocator, Song song) : base(serviceLocator)
        {
            this.song = song;

            beatSaverService = serviceLocator.GetService<IBeatSaverService>();
            songCopyDomain = serviceLocator.GetService<ISongCopyDomain>();
            songCopyDomain.OnPlaylistSelectionChanged += SongCopyDomain_OnPlaylistSelectionChanged;

            difficulties.AddRange(song.Difficulties?.Select(d => new PlaylistSongDifficultyViewModel(serviceLocator, d, true)) ?? []);
            OnPropertyChanged(nameof(Difficulties));
        }

        public void CleanUpReferences()
        {
            Difficulties.ForEach(d => d.DifficultyChanged -= DifficultyViewModel_DifficultyChanged);
            AvailableDifficulties.ForEach(d => d.DifficultyChanged -= DifficultyViewModel_DifficultyChanged);
            songCopyDomain.OnPlaylistSelectionChanged -= SongCopyDomain_OnPlaylistSelectionChanged;
        }

        public void UpdateData(MapDetail mapDetail)
        {
            song.Key = mapDetail.Id;
            song.SongName = mapDetail.Metadata?.SongName ?? string.Empty;
            song.LevelAuthorName = mapDetail.Metadata?.LevelAuthorName;
            song.LevelId = mapDetail.Id;
            Uploaded = mapDetail.Uploaded;

            OnPropertyChanged(nameof(BsrKey));
            OnPropertyChanged(nameof(BsrKeyHex));
            OnPropertyChanged(nameof(SongName));
            OnPropertyChanged(nameof(LevelAuthorName));
            OnPropertyChanged(nameof(Uploaded));

            UpdateMapDetail(mapDetail);
        }

        public void UpdateMapDetail(MapDetail mapDetail)
        {
            MapDetailViewModel = new MapDetailViewModel(ServiceLocator, mapDetail);
        }

        public void AddDifficulty(Difficulty difficulty)
        {
            var existingDifficulty = difficulties.SingleOrDefault(d => d.Characteristic == difficulty.Characteristic && d.Difficulty == difficulty.Name);
            if (existingDifficulty == null)
            {
                song.Difficulties ??= [];
                song.Difficulties.Add(difficulty);
                var newSongDifficultyViewModel = new PlaylistSongDifficultyViewModel(ServiceLocator, difficulty, true);
                difficulties.Add(newSongDifficultyViewModel);
                OnPropertyChanged(nameof(Difficulties));
            }
            else
            {
                existingDifficulty.IsSelected = true;
            }
        }

        #region Helper methods

        public async Task AddToPlaylistAsync()
        {
            if (MapDetailViewModel == null)
            {
                var mapDetail = await beatSaverService.GetMapDetailAsync(BsrKey, BeatSaverKeyType.Id);
                if (mapDetail == null)
                    return;
                UpdateMapDetail(mapDetail);
            }

            if (MapDetailViewModel == null)
                return;

            Song? songToCopy = null;
            if (MapDetailViewModel.Model.Versions.Count == 1)
            {
                songToCopy = new Song
                {
                    Hash = MapDetailViewModel.Model.Versions.First().Hash,
                    Key = MapDetailViewModel.Model.Versions.First().Key,
                    LevelAuthorName = MapDetailViewModel.Model.Metadata?.LevelAuthorName,
                    SongName = MapDetailViewModel.Model.Metadata?.SongName ?? string.Empty,
                };
            }
            else
            {
                // todo: Show version selection dialog
            }

            if (songToCopy == null)
                return;

            var songCopyEventArgs = new SongCopyEventArgs
            {
                Songs = { songToCopy }
            };
            songCopyDomain.CopySongs(songCopyEventArgs);
        }

        public bool CanAddToPlaylist()
        {
            return songCopyDomain.SelectedPlaylist is PlaylistViewModel;
        }

        private void RemoveFromPlaylist()
        {
            OnSongRemoved?.Invoke(this, EventArgs.Empty);
        }

        private bool CanRemoveFromPlaylist()
        {
            return true;
        }

        private async Task SetAvailableDifficultiesAsync()
        {
            AvailableDifficulties.ForEach(d => d.DifficultyChanged -= DifficultyViewModel_DifficultyChanged);
            AvailableDifficulties.Clear();

            var mapDetail = await beatSaverService.GetMapDetailAsync(Hash, BeatSaverKeyType.Hash);
            if (mapDetail == null)
                return;

            MapDetailViewModel ??= new MapDetailViewModel(ServiceLocator, mapDetail);

            var latestVersion = mapDetail.Versions.OrderByDescending(v => v.CreatedAt).FirstOrDefault();
            if (latestVersion == null)
                return;

            foreach (var difficulty in latestVersion.Diffs.OrderBy(d => d.Characteristic).ThenBy(d => d.Difficulty))
            {
                var difficultyViewModel = Difficulties.SingleOrDefault(d => d.Difficulty == difficulty.Difficulty && d.Characteristic == difficulty.Characteristic);
                if (difficultyViewModel == null)
                {
                    var playlistSongDifficulty = new Difficulty
                    {
                        Characteristic = difficulty.Characteristic,
                        Name = difficulty.Difficulty,
                    };
                    difficultyViewModel = new PlaylistSongDifficultyViewModel(ServiceLocator, playlistSongDifficulty);
                }

                difficultyViewModel.DifficultyChanged += DifficultyViewModel_DifficultyChanged;
                AvailableDifficulties.Add(difficultyViewModel);
            }
        }

        private bool CanSetAvailableDifficulties()
        {
            return true;
        }

        private void DifficultyViewModel_DifficultyChanged(object? sender, EventArgs e)
        {
            if (sender is not PlaylistSongDifficultyViewModel difficultyViewModel)
                return;

            if (difficultyViewModel.IsSelected)
            {
                var difficulty = new Difficulty
                {
                    Characteristic = difficultyViewModel.Characteristic,
                    Name = difficultyViewModel.Difficulty,
                };

                song.Difficulties ??= [];
                song.Difficulties.Add(difficulty);
                difficulties.Add(difficultyViewModel);
                OnPropertyChanged(nameof(Difficulties));
            }
            else
            {
                var existingDifficultyVieWModel = Difficulties?.SingleOrDefault(d => d.Characteristic == difficultyViewModel.Characteristic && d.Difficulty == difficultyViewModel.Difficulty);
                if (existingDifficultyVieWModel == null)
                    return;
                difficulties?.Remove(existingDifficultyVieWModel);
                OnPropertyChanged(nameof(Difficulties));

                var existingDifficulty = song.Difficulties?.SingleOrDefault(d => d.Characteristic == difficultyViewModel.Characteristic && d.Name == difficultyViewModel.Difficulty);
                if (existingDifficulty == null)
                    return;
                song.Difficulties?.Remove(existingDifficulty);

            }
        }

        private void SongCopyDomain_OnPlaylistSelectionChanged(object? sender, PlaylistSelectionChangedEventArgs e)
        {
            addToPlaylistCommand?.RaiseCanExecuteChanged();
        }

        #endregion
    }
}
