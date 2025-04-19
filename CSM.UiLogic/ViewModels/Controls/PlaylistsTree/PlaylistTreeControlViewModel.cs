using CSM.Business.Interfaces;
using CSM.DataAccess.Playlists;
using CSM.Framework.Extensions;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;
using CSM.UiLogic.Converter;
using CSM.UiLogic.Helper;
using CSM.UiLogic.ViewModels.Common.Playlists;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;

namespace CSM.UiLogic.ViewModels.Controls.PlaylistsTree
{
    internal class PlaylistTreeControlViewModel(IServiceLocator serviceLocator, bool hasEditHeader = true) : BaseViewModel(serviceLocator)
    {
        #region Private fields

        private BasePlaylistViewModel? selectedPlaylist;

        private IRelayCommand? addFolderCommand;
        private IRelayCommand? addPlaylistCommand;
        private IRelayCommand? deletePlaylistCommand;
        private IRelayCommand? openInFileExplorerCommand;
        private IRelayCommand? refreshCommand;

        private readonly ILogger<PlaylistTreeControlViewModel> logger = serviceLocator.GetService<ILogger<PlaylistTreeControlViewModel>>();
        private readonly IUserConfigDomain userConfigDomain = serviceLocator.GetService<IUserConfigDomain>();

        #endregion

        #region Properties

        public bool HasEditHeader => hasEditHeader;

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

            SetLoadingInProgress(true, "Loading playlists...");

            Playlists.Clear();
            var path = userConfigDomain?.Config?.PlaylistsConfig.PlaylistPath.Path;
            if (string.IsNullOrEmpty(path) || !Path.Exists(path))
            {
                SetLoadingInProgress(false, string.Empty);
                return;
            }
            Playlists.AddRange(await LoadDirectoryStructureAsync(path));

            SetLoadingInProgress(false, string.Empty);
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
                try
                {
                    var content = await File.ReadAllTextAsync(file);
                    var playList = JsonSerializer.Deserialize<Playlist>(content);
                    if (playList == null)
                        continue;
                    var playlistViewModel = new PlaylistViewModel(ServiceLocator, playList, file);
                    retval.Add(playlistViewModel);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Error loading playlist from {file}");
                    continue;
                }

            }

            return retval;
        }

        private void AddFolder()
        {
            var selectedFolderViewModel = selectedPlaylist as PlaylistFolderViewModel;
            var currentFolder = selectedFolderViewModel?.Path ?? userConfigDomain.Config?.PlaylistsConfig.PlaylistPath.Path;
            if (currentFolder == null)
                return;

            var editNewFolderName = new NewPlaylistFolderViewModel(ServiceLocator, "Cancel", EditViewModelCommandColor.Default, "Create folder", EditViewModelCommandColor.Default);
            UserInteraction.ShowWindow(editNewFolderName);
            if (editNewFolderName.Continue)
            {
                try
                {
                    var newDirectoryPath = Path.Combine(currentFolder, editNewFolderName.FolderName);
                    Directory.CreateDirectory(newDirectoryPath);
                    var newFolderViewModel = new PlaylistFolderViewModel(ServiceLocator, newDirectoryPath);
                    if (selectedFolderViewModel != null)
                        selectedFolderViewModel.Playlists.Add(newFolderViewModel);
                    else
                        Playlists.Add(newFolderViewModel);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Unable to create new folder with name '{folderName}'", editNewFolderName.FolderName);
                    MessageBox.Show($"Unable to create new folder with name '{editNewFolderName.FolderName}'", "Unable to create new folder");
                }
            }
        }

        private bool CanAddFolder()
        {
            return true;
        }

        private void AddPlaylist()
        {
            var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (assemblyLocation == null)
                return; 

            var defaultImageLocation = Path.Combine(assemblyLocation, "Images\\CSM_Logo_400px.png");
            var playlistsPath = userConfigDomain.Config?.PlaylistsConfig.PlaylistPath.Path;
            var selectedFolderViewModel = selectedPlaylist as PlaylistFolderViewModel;
            var currentFolder = selectedFolderViewModel?.Path ?? playlistsPath;
            if (currentFolder == null)
                return; 

            var editNewPlaylistName = new NewPlaylistViewModel(ServiceLocator, "Cancel", EditViewModelCommandColor.Default, "Create playlist", EditViewModelCommandColor.Default);
            UserInteraction.ShowWindow(editNewPlaylistName);
            if (editNewPlaylistName.Continue)
            {
                try
                {
                    var playlistPath = Path.Combine(currentFolder, editNewPlaylistName.PlaylistName + ".json");
                    var playlist = new Playlist
                    {
                        PlaylistTitle = editNewPlaylistName.PlaylistName,
                        PlaylistAuthor = string.Empty,
                        PlaylistDescription = string.Empty,
                        Image = $"base64,{ImageConverter.StringFromBitmap(defaultImageLocation)}"
                    };

                    var content = JsonSerializer.Serialize(playlist, JsonSerializerHelper.CreateDefaultSerializerOptions());
                    File.WriteAllText(playlistPath, content);

                    var playlistViewModel = new PlaylistViewModel(ServiceLocator, playlist, playlistPath);
                    if (selectedFolderViewModel != null)
                        selectedFolderViewModel.Playlists.Add(playlistViewModel);
                    else
                        Playlists.Add(playlistViewModel);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Unable to create new playlist with name '{playlistName}'", editNewPlaylistName.PlaylistName);
                    MessageBox.Show($"Unable to create new playlist with name '{editNewPlaylistName.PlaylistName}'", "Unable to create new playlist");
                }
            }

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
