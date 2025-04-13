using CSM.Business.Interfaces;
using CSM.DataAccess.Playlists;
using CSM.Framework.Extensions;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;
using CSM.UiLogic.ViewModels.Common.Playlists;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace CSM.UiLogic.ViewModels.Controls.PlaylistsTree
{
    internal class PlaylistTreeControlViewModel(IServiceLocator serviceLocator) : BaseViewModel(serviceLocator)
    {
        #region Private fields

        private BasePlaylistViewModel? selectedPlaylist;

        private IRelayCommand? addFolderCommand;
        private IRelayCommand? addPlaylistCommand;
        private IRelayCommand? deletePlaylistCommand;
        private IRelayCommand? openInFileExplorerCommand;
        private IRelayCommand? refreshCommand;

        private readonly IUserConfigDomain userConfigDomain = serviceLocator.GetService<IUserConfigDomain>();

        #endregion

        #region Properties

        public ObservableCollection<BasePlaylistViewModel> Playlists { get; } = [];

        public BasePlaylistViewModel? SelectedPlaylist
        {
            get => selectedPlaylist;
            set
            {
                if (value == selectedPlaylist)
                    return;
                selectedPlaylist = value;
                OnPropertyChanged();

                UpdateCommands();
            }
        }

        public IRelayCommand AddFolderCommand => addFolderCommand ??= CommandFactory.Create(AddFolder, CanAddFolder);

        public IRelayCommand AddPlaylistCommand => addPlaylistCommand ??= CommandFactory.Create(AddPlaylist, CanAddPlaylist);

        public IRelayCommand DeletePlaylistCommand => deletePlaylistCommand ??= CommandFactory.Create(Delete, CanDelete);

        public IRelayCommand OpenInFileExplorerCommand => openInFileExplorerCommand ??= CommandFactory.Create(OpenInFileExplorer, CanOpenInFileExplorer);

        public IRelayCommand RefreshCommand => refreshCommand ??= CommandFactory.CreateFromAsync(RefreshAsync, CanRefresh);

        #endregion

        public async Task LoadAsync(bool refresh)
        {
            if (Playlists.Count > 0 && !refresh)
                return;

            LoadingInProgress = true;

            Playlists.Clear();
            var path = userConfigDomain?.Config?.PlaylistsConfig.PlaylistPath.Path;
            if (string.IsNullOrEmpty(path))
                return;
            if (!Path.Exists(path))
                return;
            Playlists.AddRange(await LoadDirectoryStructureAsync(path));

            LoadingInProgress = false;
        }

        #region Helper methods

        private async Task<List<BasePlaylistViewModel>> LoadDirectoryStructureAsync(string path)
        {
            var retval = new List<BasePlaylistViewModel>();

            var directories = Directory.GetDirectories(path);
            foreach (var directory in directories.Where(d => !d.ToLower().Contains("coverimages")))
            {
                var playlistFolderViewModel = new PlaylistFolderViewModel(ServiceLocator, directory);
                playlistFolderViewModel.Playlists.AddRange(await LoadDirectoryStructureAsync(directory));
                retval.Add(playlistFolderViewModel);
            }

            var files = Directory.GetFiles(path);
            var allowedExtensions = new[] { ".json", ".bplist" };
            foreach (var file in files.Where(file => allowedExtensions.Contains(Path.GetExtension(file).ToLower())))
            {
                var content = await File.ReadAllTextAsync(file);
                var playList = JsonSerializer.Deserialize<Playlist>(content);
                if (playList == null)
                    continue;
                var playlistViewModel = new PlaylistViewModel(ServiceLocator, playList, file);
                retval.Add(playlistViewModel);
            }

            return retval;
        }

        private void AddFolder()
        {
            var selectedFolderViewModel = selectedPlaylist as PlaylistFolderViewModel;
            var currentFolder = selectedFolderViewModel?.Path ?? userConfigDomain.Config?.PlaylistsConfig.PlaylistPath.Path;


        }

        private bool CanAddFolder()
        {
            return true;
        }

        private void AddPlaylist()
        {

        }

        private bool CanAddPlaylist()
        {
            return true;
        }

        private void Delete()
        {
            if (selectedPlaylist is PlaylistViewModel playlistViewModel)
            {
                if (!File.Exists(playlistViewModel.Path))
                    return;

                File.Delete(playlistViewModel.Path);
                DeletePlaylistFromTree(null, playlistViewModel);
            }
            else if (selectedPlaylist is PlaylistFolderViewModel folderViewModel)
            {
                if (!Directory.Exists(folderViewModel.Path))
                    return;

                Directory.Delete(folderViewModel.Path, true);
                DeletePlaylistFolderFromTree(null, folderViewModel);
            }

            SelectedPlaylist = null;
        }

        private bool CanDelete()
        {
            return selectedPlaylist != null;
        }

        private void DeletePlaylistFromTree(PlaylistFolderViewModel? folder, PlaylistViewModel playlistToDelete)
        {
            var currentPlaylists = folder?.Playlists ?? Playlists;
            if (currentPlaylists.Contains(playlistToDelete))
            {
                currentPlaylists.Remove(playlistToDelete);
                return;
            }

            foreach (var playlist in currentPlaylists)
            {
                if (playlist is PlaylistFolderViewModel folderViewModel)
                {
                    DeletePlaylistFromTree(folderViewModel, playlistToDelete);
                }
            }
        }

        private void DeletePlaylistFolderFromTree(PlaylistFolderViewModel? folder, PlaylistFolderViewModel folderToDelete)
        {
            var currentPlaylists = folder?.Playlists ?? Playlists;
            if (currentPlaylists.Contains(folderToDelete))
            {
                currentPlaylists.Remove(folderToDelete);
                return;
            }

            foreach (var playlist in currentPlaylists)
            {
                if (playlist is PlaylistFolderViewModel folderViewModel)
                {
                    DeletePlaylistFolderFromTree(folderViewModel, folderToDelete);
                }
            }
        }

        private void OpenInFileExplorer()
        {
            if (selectedPlaylist == null)
                return;

            var psi = new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = $"/select,\"{selectedPlaylist.Path}\"",
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private bool CanOpenInFileExplorer()
        {
            return selectedPlaylist != null;
        }

        private async Task RefreshAsync()
        {
            await LoadAsync(true);
        }

        private bool CanRefresh()
        {
            return true;
        }

        private void UpdateCommands()
        {
            DeletePlaylistCommand.RaiseCanExecuteChanged();
            OpenInFileExplorerCommand.RaiseCanExecuteChanged();
        }

        #endregion
    }
}
