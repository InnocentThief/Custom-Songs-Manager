using CSM.Framework.ServiceLocation;
using System.Collections.ObjectModel;

namespace CSM.UiLogic.ViewModels.Common.Playlists
{
    internal sealed class PlaylistFolderViewModel(
        IServiceLocator serviceLocator,
        string path)
        : BasePlaylistViewModel(serviceLocator, System.IO.Path.GetFileName(path), path)
    {
        public ObservableCollection<BasePlaylistViewModel> Playlists { get; } = [];
    }
}
