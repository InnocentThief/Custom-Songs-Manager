using CSM.DataAccess.Playlists;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Common.Playlists
{
    internal class PlaylistSongViewModel(IServiceLocator serviceLocator, Song song)
        : BaseViewModel(serviceLocator)
    {
        private Song song = song;
    }

}
