using CSM.Business.Core.SongCopy;
using CSM.Business.Interfaces;
using CSM.DataAccess.BeatSaver;
using CSM.DataAccess.Playlists;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;
using CSM.UiLogic.ViewModels.Common.MapDetails;
using System.Globalization;

namespace CSM.UiLogic.ViewModels.Common.Playlists
{
    internal class PlaylistSongViewModel : BaseViewModel
    {
        #region Private fields

        private MapDetailViewModel? mapDetailViewModel;
        private IRelayCommand? addToPlaylistCommand, removeFromPlaylistCommand;

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

        public List<PlaylistSongDifficultyViewModel> Difficulties => [.. difficulties.OrderBy(d => d.Difficulty)];

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

        #endregion

        public event EventHandler? OnSongRemoved;

        public PlaylistSongViewModel(IServiceLocator serviceLocator, Song song) : base(serviceLocator)
        {
            this.song = song;

            beatSaverService = serviceLocator.GetService<IBeatSaverService>();
            songCopyDomain = serviceLocator.GetService<ISongCopyDomain>();
            songCopyDomain.OnPlaylistSelectionChanged += SongCopyDomain_OnPlaylistSelectionChanged;

            difficulties.AddRange(song.Difficulties?.Select(d => new PlaylistSongDifficultyViewModel(serviceLocator, d)) ?? []);
            OnPropertyChanged(nameof(Difficulties));
        }

        public void CleanUpReferences()
        {
            songCopyDomain.OnPlaylistSelectionChanged -= SongCopyDomain_OnPlaylistSelectionChanged;
        }

        public void UpdateData(MapDetail mapDetail)
        {
            song.Key = mapDetail.Id;
            song.SongName = mapDetail.Name;
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

        private void SongCopyDomain_OnPlaylistSelectionChanged(object? sender, Business.Core.SongCopy.PlaylistSelectionChangedEventArgs e)
        {
            addToPlaylistCommand?.RaiseCanExecuteChanged();
        }

        #endregion
    }
}
