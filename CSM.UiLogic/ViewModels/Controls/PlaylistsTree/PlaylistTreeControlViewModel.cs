using CSM.Business.Core.SongSelection;
using CSM.Business.Interfaces;
using CSM.DataAccess;
using CSM.DataAccess.Playlists;
using CSM.Framework.Extensions;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;
using CSM.UiLogic.Converter;
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
    internal class PlaylistTreeControlViewModel : BaseViewModel
    {
        #region Private fields

        private BasePlaylistViewModel? selectedPlaylist;

        private IRelayCommand? addFolderCommand;
        private IRelayCommand? addPlaylistCommand;
        private IRelayCommand? deletePlaylistCommand;
        private IRelayCommand? openInFileExplorerCommand;
        private IRelayCommand? refreshCommand;

        private readonly SongSelectionType songSelectionType;
        private readonly ILogger<PlaylistTreeControlViewModel> logger;
        private readonly IUserConfigDomain userConfigDomain;
        private readonly ISongCopyDomain songCopyDomain;
        private readonly ISongSelectionDomain songSelectionDomain;
        private readonly bool isReadOnly;

        #endregion

        #region Properties

        public bool IsReadOnly => isReadOnly;

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
                if (!isReadOnly)
                    songCopyDomain.SetSelectedPlaylist(value);
            }
        }

        public IRelayCommand AddFolderCommand => addFolderCommand ??= CommandFactory.Create(AddFolder, CanAddFolder);

        public IRelayCommand AddPlaylistCommand => addPlaylistCommand ??= CommandFactory.Create(AddPlaylist, CanAddPlaylist);

        public IRelayCommand DeletePlaylistCommand => deletePlaylistCommand ??= CommandFactory.Create(Delete, CanDelete);

        public IRelayCommand OpenInFileExplorerCommand => openInFileExplorerCommand ??= CommandFactory.Create(OpenInFileExplorer, CanOpenInFileExplorer);

        public IRelayCommand RefreshCommand => refreshCommand ??= CommandFactory.CreateFromAsync(RefreshAsync, CanRefresh);

        #endregion

        public PlaylistTreeControlViewModel(IServiceLocator serviceLocator, SongSelectionType songSelectionType, bool isReadOnly = false) : base(serviceLocator)
        {
            this.songSelectionType = songSelectionType;
            logger = serviceLocator.GetService<ILogger<PlaylistTreeControlViewModel>>();
            userConfigDomain = serviceLocator.GetService<IUserConfigDomain>();
            songCopyDomain = serviceLocator.GetService<ISongCopyDomain>();
            songSelectionDomain = serviceLocator.GetService<ISongSelectionDomain>();
            this.isReadOnly = isReadOnly;

            if (!isReadOnly)
                songCopyDomain.OnCreatePlaylist += SongCopyDomain_OnCreatePlaylist;
            songSelectionDomain.OnSongSelectionChanged += SongSelectionDomain_OnSongSelectionChanged;
        }

        public async Task LoadAsync(bool refresh)
        {
            if (Playlists.Count > 0 && !refresh)
                return;

            SetLoadingInProgress(true, "Loading playlists...");

            Playlists.ForEach(pl => pl.CleanUpReferences());
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
            foreach (var directory in directories.Where(d => !d.Contains("coverimages", StringComparison.CurrentCultureIgnoreCase)))
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
                    var playList = JsonSerializer.Deserialize<Playlist>(content, JsonSerializerHelper.CreateDefaultSerializerOptions());
                    if (playList == null)
                        continue;
                    var playlistViewModel = new PlaylistViewModel(ServiceLocator, playList, file, songSelectionType, isReadOnly);
                    retval.Add(playlistViewModel);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error loading playlist from {file}", file);
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

            var editNewPlaylistName = new NewPlaylistViewModel(ServiceLocator, "Cancel", EditViewModelCommandColor.Default, "Create playlist", EditViewModelCommandColor.Default);
            UserInteraction.ShowWindow(editNewPlaylistName);
            if (editNewPlaylistName.Continue)
            {
                CreatePlaylist(editNewPlaylistName.PlaylistName, [], defaultImageLocation);
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

        private void SongCopyDomain_OnCreatePlaylist(object? sender, Business.Core.SongCopy.CreatePlaylistEventArgs e)
        {
            var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (assemblyLocation == null)
                return;

            CreatePlaylist(e.PlaylistName, e.Songs, Path.Combine(assemblyLocation, "Images\\CSM_Logo_Song_Suggest_400px.png"));
        }

        private void SongSelectionDomain_OnSongSelectionChanged(object? sender, SongSelectionChangedEventArgs e)
        {
            foreach (var playlist in Playlists)
            {
                playlist.CheckContainsSong(e.SongHash, e.SongSelectionType);
            }
        }

        private void CreatePlaylist(string playlistName, List<Song> songs, string? image)
        {
            var playlistsPath = userConfigDomain.Config?.PlaylistsConfig.PlaylistPath.Path;
            var selectedFolderViewModel = selectedPlaylist as PlaylistFolderViewModel;
            var currentFolder = selectedFolderViewModel?.Path ?? playlistsPath;
            if (currentFolder == null)
                return;

            try
            {
                var playlistPath = Path.Combine(currentFolder, playlistName + ".json");
                var playlist = new Playlist
                {
                    PlaylistTitle = playlistName,
                    PlaylistAuthor = string.Empty,
                    PlaylistDescription = string.Empty,
                    Songs = [],
                    Image = image != null ? $"base64,{ImageConverter.StringFromBitmap(image)}" : string.Empty,
                };

                foreach (var songToCopy in songs)
                {
                    var existingSong = playlist.Songs.SingleOrDefault(s => s.Hash == songToCopy.Hash);
                    if (existingSong != null)
                    {
                        foreach (var difficultyToCopy in songToCopy.Difficulties ?? [])
                        {
                            var existingDifficulty = existingSong.Difficulties.SingleOrDefault(d => d.Characteristic == difficultyToCopy.Characteristic && d.Name == difficultyToCopy.Name);
                            if (existingDifficulty == null)
                            {
                                existingSong.Difficulties ??= [];
                                existingSong.Difficulties.Add(difficultyToCopy);
                            }
                        }
                    }
                    else
                    {
                        playlist.Songs.Add(songToCopy);
                    }
                }

                var content = JsonSerializer.Serialize(playlist, JsonSerializerHelper.CreateDefaultSerializerOptions());
                File.WriteAllText(playlistPath, content);

                var playlistViewModel = new PlaylistViewModel(ServiceLocator, playlist, playlistPath, songSelectionType, IsReadOnly);
                if (selectedFolderViewModel != null)
                    selectedFolderViewModel.Playlists.Add(playlistViewModel);
                else
                    Playlists.Add(playlistViewModel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to create new playlist with name '{playlistName}'", playlistName);
                MessageBox.Show($"Unable to create new playlist with name '{playlistName}'", "Unable to create new playlist");
            }
        }

        #endregion
    }
}
