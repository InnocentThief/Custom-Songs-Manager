using CSM.Business.Core.SongSelection;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.ViewModels.Controls.PlaylistsTree;

namespace CSM.UiLogic.ViewModels.Workspaces
{
    internal sealed class TwitchWorkspaceViewModel : WorkspaceViewModel
    {
        public override string Title => "Custom Songs Manager - Twitch";

        public PlaylistTreeControlViewModel PlaylistsTree { get; }

        public TwitchWorkspaceViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
            PlaylistsTree = new PlaylistTreeControlViewModel(ServiceLocator, SongSelectionType.Left, true);
        }

        public override async Task ActivateAsync(bool refresh)
        {
            var playlistTreeTask = PlaylistsTree.LoadAsync(refresh);

            await Task.WhenAll(playlistTreeTask);
        }
    }
}
