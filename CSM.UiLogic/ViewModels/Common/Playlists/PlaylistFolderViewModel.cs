using System.Collections.ObjectModel;
using CSM.Business.Core.SongSelection;
using CSM.Business.Interfaces.SongCopy;
using CSM.Framework.ServiceLocation;

namespace CSM.UiLogic.ViewModels.Common.Playlists
{
    internal sealed class PlaylistFolderViewModel(
        IServiceLocator serviceLocator,
        string path)
        : BasePlaylistViewModel(serviceLocator, System.IO.Path.GetFileName(path), path), IPlaylistFolderViewModel
    {
        public ObservableCollection<BasePlaylistViewModel> Playlists { get; } = [];

        public override bool CheckContainsSong(string? hash, SongSelectionType songSelectionType)
        {
            var retval = false;

            foreach (var playlist in Playlists)
            {
                var contains = playlist.CheckContainsSong(hash, songSelectionType);
                if (retval == false)
                    retval = contains;
            }

            if (songSelectionType == SongSelectionType.Left)
            {
                ContainsLeftSong = retval;
            }
            else
            {
                ContainsRightSong = retval;
            }

            return retval;
        }

        public override void CleanUpReferences()
        {
            foreach (var playlist in Playlists)
            {
                playlist.CleanUpReferences();
            }
        }
    }
}
