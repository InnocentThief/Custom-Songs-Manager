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
           PlaylistsTree = new PlaylistTreeControlViewModel(ServiceLocator);
            SongSources = new SongSourcesControlViewModel(ServiceLocator);
        }

        public override async Task ActivateAsync(bool refresh)
        {
            await PlaylistsTree.LoadAsync(refresh);
            await SongSources.LoadAsync(refresh);
        }

        #region Helper methods

        //private async Task LoadPlaylistsAsync()
        //{
        //    LoadingInProgress = true;

        //    var path = "C:\\Users\\DanielKrebser\\Downloads\\Playlists";
        //    var playlists = await LoadDirectoryStructureAsync(path, null);
        //    Playlists.AddRange(playlists);

        //    LoadingInProgress = false;
        //}

        //private async Task<List<BasePlaylistViewModel>> LoadDirectoryStructureAsync(string path, PlaylistFolderViewModel? folder)
        //{
        //    var playlists = new List<BasePlaylistViewModel>();

        //    var directories = Directory.GetDirectories(path);
        //    foreach (var directory in directories)
        //    {
        //        var playlistFolder = new PlaylistFolderViewModel(ServiceLocator, directory);
                


        //        var subDirectories = await Task.WhenAll(loadSubDirectoriesTasks);

        //        if (folder != null)
        //        {
        //            folder.Playlists.Add(playlistFolder);
        //        }
        //        else
        //        {
        //            Playlists.Add(playlistFolder);
        //        }
        //    }
             
        //    Playlists.AddRange(subDirectories);

        //    var files = Directory.GetFiles(path);





        //    return playlists;
        //}

        

        #endregion
    }
}
