using System.Globalization;
using CSM.Business.Interfaces;
using CSM.DataAccess.BeatSaver;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;
using CSM.UiLogic.ViewModels.Common.MapDetails;
using CSM.UiLogic.ViewModels.Common.Playlists;

namespace CSM.UiLogic.ViewModels.Controls.SongSources.SongSearch
{
    internal class SearchResultMapDetailViewModel : BaseViewModel
    {
        #region Private fields

        private IRelayCommand? addToPlaylistCommand;
        private MapDetailViewModel? mapDetailViewModel;

        private readonly MapDetail mapDetail;
        private readonly ISongCopyDomain songCopyDomain;

        #endregion

        #region Properties

        public MapDetail Model => mapDetail;

        public IRelayCommand? AddToPlaylistCommand => addToPlaylistCommand ??= CommandFactory.Create(AddToPlaylist, CanAddToPlaylist);

        public string BsrKey => mapDetail.Id;

        public int BsrKeyHex
        {
            get
            {
                int.TryParse(BsrKey, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int result);
                return result;
            }
        }

        public string SongName => mapDetail.Metadata?.SongName ?? string.Empty;

        public string SongSubName => mapDetail.Metadata?.SongSubName ?? string.Empty;

        public string LevelAuthorName => mapDetail.Metadata?.LevelAuthorName ?? string.Empty;

        public string SongAuthorName => mapDetail.Metadata?.SongAuthorName ?? string.Empty;

        public string BPM
        {
            get
            {
                var bpm = mapDetail.Metadata?.Bpm ?? 0;
                if (bpm > 0)
                    return bpm.ToString("N0");
                return string.Empty;
            }
        }

        public string Duration
        {
            get
            {
                var duration = mapDetail.Metadata?.Duration ?? 0;

                if (duration > 0)
                {
                    TimeSpan timeSpan = TimeSpan.FromSeconds(duration);
                    return timeSpan.ToString();
                }
                return string.Empty;
            }
        }

        public DateTime Uploaded => mapDetail.Uploaded;

        public string Ranked => mapDetailViewModel?.Ranked ?? string.Empty;

        public List<MapDifficultyViewModel> Difficulties { get; } = [];

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

        public SearchResultMapDetailViewModel(IServiceLocator serviceLocator, MapDetail mapDetail) : base(serviceLocator)
        {
            this.mapDetail = mapDetail;
            MapDetailViewModel = new MapDetailViewModel(serviceLocator, mapDetail);
            songCopyDomain = serviceLocator.GetService<ISongCopyDomain>();
            songCopyDomain.OnPlaylistSelectionChanged += SongCopyDomain_OnPlaylistSelectionChanged;

            var mapVersions = mapDetail.Versions.OrderByDescending(v => v.CreatedAt).ToList();
            foreach (var mapVersion in mapVersions)
            {
                Difficulties.AddRange(mapVersion.Diffs.Select(d => new MapDifficultyViewModel(serviceLocator, d, mapVersion)));
            }
        }

        public void CleanUpReferences()
        {
            songCopyDomain.OnPlaylistSelectionChanged -= SongCopyDomain_OnPlaylistSelectionChanged;
        }

        #region Helper methods

        private void AddToPlaylist()
        {
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
