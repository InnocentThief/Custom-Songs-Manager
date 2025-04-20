using CSM.Business.Core.SongSelection;
using CSM.Business.Interfaces.SongCopy;
using CSM.Framework.ServiceLocation;
using System.Collections.ObjectModel;

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
                if (playlist.CheckContainsSong(hash, songSelectionType))
                {
                    if (songSelectionType == SongSelectionType.Left)
                        ContainsLeftSong = true;
                    else
                        ContainsRightSong = true;
                    retval =  true;
                }
                else
                {
                    if (songSelectionType == SongSelectionType.Left)
                        ContainsLeftSong = false;
                    else
                        ContainsRightSong = false;
                }
            }
            return retval;
        }
    }
}
