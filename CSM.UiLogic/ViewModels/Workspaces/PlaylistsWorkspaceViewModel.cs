using CSM.Business.Core.SongSelection;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.ViewModels.Controls.PlaylistsTree;
using CSM.UiLogic.ViewModels.Controls.SongSources;

namespace CSM.UiLogic.ViewModels.Workspaces
{
    internal sealed class PlaylistsWorkspaceViewModel : WorkspaceViewModel
    {
        public override string Title => "Custom Songs Manager - Playlists";

        public PlaylistTreeControlViewModel PlaylistsTree { get; }

        public SongSourcesControlViewModel SongSources { get; }

        public PlaylistsWorkspaceViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
            PlaylistsTree = new PlaylistTreeControlViewModel(ServiceLocator, SongSelectionType.Left);
            SongSources = new SongSourcesControlViewModel(ServiceLocator);
        }

        public override async Task ActivateAsync(bool refresh)
        {
            var playlistTreeTask = PlaylistsTree.LoadAsync(refresh);
            var songSourcesTask = SongSources.LoadAsync();

            await Task.WhenAll(playlistTreeTask, songSourcesTask);
        }
    }
}
