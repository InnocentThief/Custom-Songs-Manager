using CSM.Business.Core.SongCopy;
using CSM.Business.Interfaces;
using CSM.DataAccess.BeatSaver;
using CSM.DataAccess.Playlists;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;
using CSM.UiLogic.ViewModels.Common.MapDetails;
using CSM.UiLogic.ViewModels.Common.Playlists;

namespace CSM.UiLogic.ViewModels.Controls.SongSources.BeatSaberFavourites
{
    internal class BeatSaberFavouriteViewModel : BaseViewModel
    {
        #region Private fields

        private IRelayCommand? addToPlaylistCommand;

        private readonly ISongCopyDomain songCopyDomain;

        #endregion

        #region Properties

        public IRelayCommand AddToPlaylistCommand => addToPlaylistCommand ??= CommandFactory.Create(AddToPlaylist, CanAddToPlaylist);

        public MapDetailViewModel MapDetailViewModel { get; }

        #endregion

        public BeatSaberFavouriteViewModel(IServiceLocator serviceLocator, MapDetail mapDetail) : base(serviceLocator)
        {
            MapDetailViewModel = new MapDetailViewModel(serviceLocator, mapDetail);

            songCopyDomain = serviceLocator.GetService<ISongCopyDomain>();
            songCopyDomain.OnPlaylistSelectionChanged += SongCopyDomain_OnPlaylistSelectionChanged;
        }

        public void CleanUpReferences()
        {
            songCopyDomain.OnPlaylistSelectionChanged -= SongCopyDomain_OnPlaylistSelectionChanged;
        }

        #region Helper methods

        private void AddToPlaylist()
        {
            Song? songToCopy = new Song
            {
                Hash = MapDetailViewModel.Model.Versions.First().Hash,
                Key = MapDetailViewModel.Id,
                LevelAuthorName = MapDetailViewModel.Model.Metadata?.LevelAuthorName,
                SongName = MapDetailViewModel.Model.Metadata?.SongName ?? string.Empty
            };

            var songcopyEventArgs = new SongCopyEventArgs
            {
                Songs = { songToCopy }
            };
            songCopyDomain.CopySongs(songcopyEventArgs);
        }

        private bool CanAddToPlaylist()
        {
            return songCopyDomain.SelectedPlaylist is PlaylistViewModel;
        }

        private void SongCopyDomain_OnPlaylistSelectionChanged(object? sender, PlaylistSelectionChangedEventArgs e)
        {
            addToPlaylistCommand?.RaiseCanExecuteChanged();
        }

        #endregion
    }
}
