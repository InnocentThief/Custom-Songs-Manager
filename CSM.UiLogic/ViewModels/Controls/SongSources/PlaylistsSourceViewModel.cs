using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.ViewModels.Controls.PlaylistsTree;

namespace CSM.UiLogic.ViewModels.Controls.SongSources
{
    internal class PlaylistsSourceViewModel : BaseViewModel, ISongSourceViewModel
    {
        public PlaylistTreeControlViewModel PlaylistsTree { get; }

        public PlaylistsSourceViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
            PlaylistsTree = new PlaylistTreeControlViewModel(ServiceLocator, false);
        }

        public async Task LoadAsync()
        {
            await PlaylistsTree.LoadAsync(false);
        }
    }
}
