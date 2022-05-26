using CSM.DataAccess.Entities.Offline;
using CSM.Framework;
using CSM.Framework.Configuration.UserConfiguration;
using CSM.Framework.Extensions;
using CSM.Framework.Logging;
using CSM.UiLogic.Properties;
using CSM.UiLogic.Wizards;
using CSM.UiLogic.Workspaces.Playlists;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;

namespace CSM.UiLogic.Workspaces
{
    /// <summary>
    /// ViewModel for the Playlists workspace.
    /// </summary>
    internal class PlaylistsViewModel : BaseWorkspaceViewModel
    {
        #region Private fields

        private BasePlaylistViewModel selectedPlaylist;
        private string playlistPath;
        private BackgroundWorker bgWorker;
        private bool isLoading;
        private int loadProgress;
        private int playlistCount;
        private PlaylistSelectionState playlistSelectionState;

        #endregion

        #region Public Properties

        /// <summary>
        /// Contains all available playlists.
        /// </summary>
        public ObservableCollection<BasePlaylistViewModel> Playlists { get; }

        /// <summary>
        /// Gets or sets the currently selected playlist.
        /// </summary>
        public BasePlaylistViewModel SelectedPlaylist
        {
            get => selectedPlaylist;
            set
            {
                if (value == selectedPlaylist) return;
                selectedPlaylist = value;
                OnPropertyChanged();
                DeletePlaylistCommand.NotifyCanExecuteChanged();
                foreach (var playlist in Playlists)
                {
                    playlist.CheckContainsSong(String.Empty);
                }
                playlistSelectionState.PlaylistSelectionChanged(selectedPlaylist != null && selectedPlaylist.GetType() != typeof(PlaylistFolderViewModel));
            }
        }

        /// <summary>
        /// Command used to refresh the workspace data.
        /// </summary>
        public RelayCommand RefreshCommand { get; }

        /// <summary>
        /// Command used to add a new folder to the playlist directory.
        /// </summary>
        public RelayCommand AddFolderCommand { get; }

        /// <summary>
        /// Command used to add a new playlist to the selected folder.
        /// </summary>
        public RelayCommand AddPlaylistCommand { get; }

        /// <summary>
        /// Command used to delete the selected custom level.
        /// </summary>
        public RelayCommand DeletePlaylistCommand { get; }

        /// <summary>
        /// Gets or sets whether the data is loading.
        /// </summary>
        public bool IsLoading
        {
            get => isLoading;
            set
            {
                if (value == isLoading) return;
                isLoading = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the load progress.
        /// </summary>
        public int LoadProgress
        {
            get => loadProgress;
            set
            {
                loadProgress = (int)(100.0 / playlistCount * value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the custom level path.
        /// </summary>
        public string PlaylistPath
        {
            get => playlistPath;
            set
            {
                if (playlistPath == value) return;
                playlistPath = value;
                OnPropertyChanged();
            }
        }

        public PlaylistCustomLevelsViewModel CustomLevels { get; }

        /// <summary>
        /// Gets the workspace type.
        /// </summary>
        public override WorkspaceType WorkspaceType => WorkspaceType.Playlists;

        #endregion

        /// <summary>
        /// Initializes a new <see cref="PlaylistViewModel"/>.
        /// </summary>
        public PlaylistsViewModel()
        {
            PlaylistPath = UserConfigManager.Instance.Config.PlaylistPaths.First().Path;
            Playlists = new ObservableCollection<BasePlaylistViewModel>();
            RefreshCommand = new RelayCommand(Refresh);
            AddFolderCommand = new RelayCommand(AddFolder);
            AddPlaylistCommand = new RelayCommand(AddPlaylist);
            DeletePlaylistCommand = new RelayCommand(DeletePlaylist, CanDeletePlaylist);
            UserConfigManager.UserConfigChanged += UserConfigManager_UserConfigChanged;
            playlistSelectionState = new PlaylistSelectionState();
            CustomLevels = new PlaylistCustomLevelsViewModel(playlistSelectionState);
            CustomLevels.AddSongToPlaylistEvent += CustomLevels_AddSongToPlaylistEvent;
        }

        /// <summary>
        /// Used to load the workspace data.
        /// </summary>
        public override void LoadData()
        {
            base.LoadData();
            PlaylistPath = UserConfigManager.Instance.Config.PlaylistPaths.First().Path;

            bgWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            bgWorker.DoWork += BackgroundWorker_DoWork;
            bgWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            bgWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            bgWorker.RunWorkerAsync();

            CustomLevels.LoadData();
        }

        /// <summary>
        /// Used to unload the workspace data.
        /// </summary>
        public override void UnloadData()
        {
            base.UnloadData();
            if (bgWorker != null && bgWorker.IsBusy) bgWorker.CancelAsync();
            Playlists.Clear();
            PlaylistPath = null;
        }

        #region Helper methods

        private void CustomLevels_AddSongToPlaylistEvent(object sender, AddSongToPlaylistEventArgs e)
        {
            ((PlaylistViewModel)SelectedPlaylist).AddPlaylistSong(e);
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Playlists.AddRange((List<BasePlaylistViewModel>)e.Result);
            IsLoading = false;

            if (bgWorker != null)
            {
                bgWorker.DoWork -= BackgroundWorker_DoWork;
                bgWorker.ProgressChanged -= BackgroundWorker_ProgressChanged;
                bgWorker.RunWorkerCompleted -= BackgroundWorker_RunWorkerCompleted;
                bgWorker.Dispose();
                bgWorker = null;
            }
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            LoadProgress = e.ProgressPercentage;
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                IsLoading = true;

                var i = 0;
                var playlists = new List<BasePlaylistViewModel>();

                if (!Directory.Exists(PlaylistPath)) return;

                playlistCount = Directory.GetDirectories(PlaylistPath).Count();
                playlistCount += Directory.GetFiles(PlaylistPath).Count();

                IEnumerable<string> folderEntries = Directory.EnumerateDirectories(PlaylistPath);
                foreach (string folderEntry in folderEntries)
                {
                    if (bgWorker.CancellationPending) return;
                    var directory = new DirectoryInfo(folderEntry);
                    if (directory.Name == "CoverImages") continue;
                    var playListFolder = new PlaylistFolderViewModel(folderEntry);
                    GetDirectoriesRecursive(playListFolder);
                    playlists.Add(playListFolder);
                    i++;
                    bgWorker.ReportProgress(i);
                }

                IEnumerable<string> files = Directory.EnumerateFiles(PlaylistPath);
                foreach (string file in files)
                {
                    if (bgWorker.CancellationPending) return;
                    if (Path.GetExtension(file) == ".json" || Path.GetExtension(file) == ".bplist")
                    {
                        var infoContent = File.ReadAllText(file);
                        Playlist playlist = JsonSerializer.Deserialize<Playlist>(infoContent);
                        if (playlist != null)
                        {
                            playlist.Path = file;
                            var playListViewModel = new PlaylistViewModel(playlist);
                            playListViewModel.SongChangedEvent += PlayListViewModel_SongChangedEvent;
                            playlists.Add(playListViewModel);
                            i++;
                            bgWorker.ReportProgress(i);
                        }
                    }
                }

                e.Result = playlists;
            }
            catch (Exception ex)
            {
                LoggerProvider.Logger.Error<CustomLevelsViewModel>($"Unable to load playlists: {ex}");
            }

        }

        private void PlayListViewModel_SongChangedEvent(object sender, PlaylistSongChangedEventArgs e)
        {
            foreach (var playlist in Playlists)
            {
                playlist.CheckContainsSong(e.Hash);
            }
        }

        private void GetDirectoriesRecursive(PlaylistFolderViewModel folder)
        {
            IEnumerable<string> folderEntries = Directory.EnumerateDirectories(folder.FilePath);
            foreach (string folderEntry in folderEntries)
            {
                var directory = new DirectoryInfo(folderEntry);
                var playListFolder = new PlaylistFolderViewModel(folderEntry);
                GetDirectoriesRecursive(playListFolder);
                folder.Playlists.Add(playListFolder);
            }

            IEnumerable<string> files = Directory.EnumerateFiles(folder.FilePath);
            foreach (string file in files)
            {
                if (Path.GetExtension(file) == ".json" || Path.GetExtension(file) == ".bplist")
                {
                    var infoContent = File.ReadAllText(file);
                    Playlist playlist = JsonSerializer.Deserialize<Playlist>(infoContent);
                    if (playlist != null)
                    {
                        playlist.Path = file;
                        var playlistViewModel = new PlaylistViewModel(playlist);
                        playlistViewModel.SongChangedEvent += PlayListViewModel_SongChangedEvent;
                        folder.Playlists.Add(playlistViewModel);
                    }
                }
            }
        }

        private void Refresh()
        {
            Playlists.Clear();
            LoadData();
        }

        private void UserConfigManager_UserConfigChanged(object sender, UserConfigChangedEventArgs e)
        {
            if (e.PlaylistsPathChanged)
            {
                PlaylistPath = UserConfigManager.Instance.Config.PlaylistPaths.First().Path;
                if (IsActive) Refresh();
            }
            if (e.CustomLevelsPathChanged)
            {
                CustomLevels.LoadData();
            }
        }

        private void DeletePlaylist()
        {
            if (SelectedPlaylist == null) return;

            if (SelectedPlaylist is PlaylistViewModel playlistViewModel)
            {
                if (File.Exists(playlistViewModel.FilePath))
                {
                    var messageBoxViewModel = new MessageBoxViewModel(Resources.Playlists_DeletePlaylist_Caption, MessageBoxButtonColor.Attention, Resources.Cancel, MessageBoxButtonColor.Default)
                    {
                        Title = Resources.Playlists_DeletePlaylist_Caption,
                        Message = Resources.Playlists_DeletePlaylist_Content,
                        MessageBoxType = DataAccess.Entities.Types.MessageBoxTypes.Question
                    };
                    MessageBoxController.Instance().ShowMessageBox(messageBoxViewModel);
                    if (messageBoxViewModel.Continue)
                    {
                        File.Delete(playlistViewModel.FilePath);
                        SelectedPlaylist.SongChangedEvent -= PlayListViewModel_SongChangedEvent;
                        DeletePlaylistRecursive(Playlists, playlistViewModel);
                        Playlists.Remove(SelectedPlaylist);
                    }
                }
            }
            else if (SelectedPlaylist is PlaylistFolderViewModel playlistFolderViewModel)
            {
                if (Directory.Exists(playlistFolderViewModel.FilePath))
                {
                    var messageBoxViewModel = new MessageBoxViewModel(Resources.Playlists_DeletePlaylistFolder_Caption, MessageBoxButtonColor.Attention, Resources.Cancel, MessageBoxButtonColor.Default)
                    {
                        Title = Resources.Playlists_DeletePlaylistFolder_Caption,
                        Message = Resources.Playlists_DeletePlaylistFolder_Content,
                        MessageBoxType = DataAccess.Entities.Types.MessageBoxTypes.Question
                    };
                    MessageBoxController.Instance().ShowMessageBox(messageBoxViewModel);
                    if (messageBoxViewModel.Continue)
                    {
                        Directory.Delete(playlistFolderViewModel.FilePath, true);
                        Playlists.Remove(SelectedPlaylist);
                    }
                }
            }
        }

        private bool CanDeletePlaylist()
        {
            return SelectedPlaylist != null;
        }

        private void DeletePlaylistRecursive(ObservableCollection<BasePlaylistViewModel> allPlaylists, PlaylistViewModel selectedPlaylist)
        {
            if (allPlaylists.Contains(selectedPlaylist))
            {
                allPlaylists.Remove(selectedPlaylist);
            }
            else
            {
                foreach (var playlist in allPlaylists)
                {
                    if (playlist is PlaylistFolderViewModel playlistFolderVieWModel)
                    {
                        DeletePlaylistRecursive(playlistFolderVieWModel.Playlists, selectedPlaylist);
                    }
                }
            }
        }

        private void AddFolder()
        {
            var playlistsPath = UserConfigManager.Instance.Config.PlaylistPaths.First().Path;
            var selectedFolder = SelectedPlaylist as PlaylistFolderViewModel;
            if (selectedFolder != null) playlistsPath = selectedFolder.FilePath;

            string input = Interaction.InputBox("Enter the name of the new folder", "Add new folder");
            if (string.IsNullOrEmpty(input)) return;
            try
            {
                var newDirectoryPath = Path.Combine(playlistPath, input);
                Directory.CreateDirectory(newDirectoryPath);

                var playlistFolderviewModel = new PlaylistFolderViewModel(newDirectoryPath);
                if (selectedFolder != null)
                {
                    selectedFolder.Playlists.Add(playlistFolderviewModel);
                }
                else
                {
                    Playlists.Add(playlistFolderviewModel);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("The name for the new folder is not valid", "Add new folder");
                return;
            }
        }

        private void AddPlaylist()
        {
            var playlistsPath = UserConfigManager.Instance.Config.PlaylistPaths.First().Path;
            var selectedFolder = SelectedPlaylist as PlaylistFolderViewModel;
            if (selectedFolder != null) playlistsPath = selectedFolder.FilePath;
            string input = Interaction.InputBox("Enter the name of the new playlist", "Add new playlist");
            if (string.IsNullOrEmpty(input)) return;
            try
            {
                var playlistPath = Path.Combine(playlistsPath, $"{input}.json");

                var playlist = new Playlist
                {
                    Path = playlistPath,
                    PlaylistAuthor = String.Empty,
                    PlaylistDescription = String.Empty,
                    PlaylistTitle = input,
                    Songs = new List<PlaylistSong>()
                };

                // Save to file
                var options = new JsonSerializerOptions { WriteIndented = true };
                var content = JsonSerializer.Serialize(playlist, options);
                File.WriteAllText(playlistPath, content);


                var playlistViewModel = new PlaylistViewModel(playlist);
                playlistViewModel.SongChangedEvent += PlayListViewModel_SongChangedEvent;
                if (selectedFolder != null)
                {
                    selectedFolder.Playlists.Add(playlistViewModel);
                }
                else
                {
                    Playlists.Add(playlistViewModel);
                }
                SelectedPlaylist = playlistViewModel;
            }
            catch (Exception)
            {
                MessageBox.Show("The name for the new playlist is not valid", "Add new folder");
                return;
            }
        }

        #endregion
    }
}