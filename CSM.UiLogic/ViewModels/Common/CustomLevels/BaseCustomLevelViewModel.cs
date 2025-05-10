using System.Globalization;
using CSM.Business.Core.SongCopy;
using CSM.Business.Interfaces;
using CSM.DataAccess.BeatSaver;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;
using CSM.UiLogic.ViewModels.Common.MapDetails;
using CSM.UiLogic.ViewModels.Common.Playlists;

namespace CSM.UiLogic.ViewModels.Common.CustomLevels
{
    internal abstract class BaseCustomLevelViewModel<TModel> : BaseViewModel, ICustomLevelViewModel where TModel : class
    {
        #region Private fields

        private MapDetailViewModel? mapDetailViewModel;
        private IRelayCommand? addToPlaylistCommand;

        private readonly IBeatSaverService beatSaverService;
        private readonly ISongCopyDomain songCopyDomain;

        #endregion

        protected TModel Model { get; }

        #region Properties

        public string Path { get; }

        public string BsrKey { get; }

        public int BsrKeyHex
        {
            get
            {
                int.TryParse(BsrKey, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int result);
                return result;
            }
        }

        public DateTime LastWriteTime { get; }

        public abstract string Version { get; }

        public abstract string SongTitle { get; }

        public abstract string SongSubTitle { get; }

        public abstract string SongAuthor { get; }

        public abstract string LevelAuthor { get; }

        public abstract double Bpm { get; }

        public abstract bool HasEasyMap { get; }

        public abstract bool HasNormalMap { get; }

        public abstract bool HasHardMap { get; }

        public abstract bool HasExpertMap { get; }

        public abstract bool HasExpertPlusMap { get; }

        public IRelayCommand AddToPlaylistCommand => addToPlaylistCommand ??= CommandFactory.CreateFromAsync(AddToPlaylistAsync, CanAddToPlaylist);

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

        #endregion

        public BaseCustomLevelViewModel(
            IServiceLocator serviceLocator,
            TModel model,
            string path,
            string bsrKey,
            DateTime lastwriteTime) : base(serviceLocator)
        {
            Model = model;
            Path = path;
            BsrKey = bsrKey;
            LastWriteTime = lastwriteTime;

            beatSaverService = serviceLocator.GetService<IBeatSaverService>();
            songCopyDomain = serviceLocator.GetService<ISongCopyDomain>();
            songCopyDomain.OnPlaylistSelectionChanged += SongCopyDomain_OnPlaylistSelectionChanged;
        }

        public void CleanUpReferences()
        {
            songCopyDomain.OnPlaylistSelectionChanged -= SongCopyDomain_OnPlaylistSelectionChanged;
        }

        public void UpdateMapDetail(MapDetail mapDetail)
        {
            MapDetailViewModel = new MapDetailViewModel(ServiceLocator, mapDetail);
        }

        #region Helper methods

        private async Task AddToPlaylistAsync()
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

            DataAccess.Playlists.Song? songToCopy = null;
            if (MapDetailViewModel.Model.Versions.Count == 1)
            {
                songToCopy = new DataAccess.Playlists.Song
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

        private bool CanAddToPlaylist()
        {
            return songCopyDomain.SelectedPlaylist is PlaylistViewModel;
        }

        private void SongCopyDomain_OnPlaylistSelectionChanged(object? sender, Business.Core.SongCopy.PlaylistSelectionChangedEventArgs e)
        {
            addToPlaylistCommand?.RaiseCanExecuteChanged();
        }

        #endregion
    }
}
