using CSM.Business.Core.SongCopy;
using CSM.Business.Interfaces;
using CSM.DataAccess.Common;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;
using CSM.UiLogic.ViewModels.Common.Playlists;

namespace CSM.UiLogic.ViewModels.Common.Leaderboard
{
    internal abstract class BaseScoreViewModel : BaseViewModel
    {
        #region Private fields

        private IRelayCommand? addToPlaylistCommand;

        protected readonly ISongCopyDomain songCopyDomain;

        #endregion

        #region Properties

        public abstract int Rank { get; }
        public abstract string Id { get; }
        public abstract string SongName { get; }
        public abstract string SubName { get; }
        public abstract string Author { get; }
        public abstract string Mapper { get; }
        public abstract string CoverImage { get; }
        public abstract bool FC { get; }
        public abstract int BadCuts { get; }
        public abstract int MissedNotes { get; }
        public abstract double Accuracy { get; }
        public string AccuracyString => $"{Accuracy}%";
        public abstract double PP { get; }
        public abstract double WeightedPP { get; }
        public abstract int Pauses { get; }
        public abstract string Modifiers { get; }
        public abstract DateTime Date { get; }
        public abstract Characteristic Characteristic { get; }
        public abstract Difficulty Difficulty { get; }

        public IRelayCommand? AddToPlaylistCommand => addToPlaylistCommand ??= CommandFactory.Create(AddToPlaylist, CanAddToPlaylist);

        #endregion

        protected BaseScoreViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
            songCopyDomain = serviceLocator.GetService<ISongCopyDomain>();
            songCopyDomain.OnPlaylistSelectionChanged += SongCopyDomain_OnPlaylistSelectionChanged;
        }


        #region Helper methods

        public abstract void AddToPlaylist();
        //{
        //if (MapDetailViewModel == null)
        //{
        //    var mapDetail = await beatSaverService.GetMapDetailAsync(BsrKey, BeatSaverKeyType.Id);
        //    if (mapDetail == null)
        //        return;
        //    UpdateMapDetail(mapDetail);
        //}

        //if (MapDetailViewModel == null)
        //    return;

        //Song? songToCopy = null;
        //if (MapDetailViewModel.Model.Versions.Count == 1)
        //{
        //    songToCopy = new Song
        //    {
        //        Hash = MapDetailViewModel.Model.Versions.First().Hash,
        //        Key = MapDetailViewModel.Model.Versions.First().Key,
        //        LevelAuthorName = MapDetailViewModel.Model.Metadata?.LevelAuthorName,
        //        SongName = MapDetailViewModel.Model.Metadata?.SongName ?? string.Empty,
        //    };
        //}
        //else
        //{
        //    // todo: Show version selection dialog
        //}

        //if (songToCopy == null)
        //    return;

        //var songCopyEventArgs = new SongCopyEventArgs
        //{
        //    Songs = { songToCopy }
        //};
        //songCopyDomain.CopySongs(songCopyEventArgs);
        //}

        public bool CanAddToPlaylist()
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
