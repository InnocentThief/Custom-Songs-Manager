using CSM.DataAccess.Entities.Offline;
using CSM.Framework;
using CSM.Framework.Configuration.UserConfiguration;
using CSM.Framework.Extensions;
using CSM.Framework.Logging;
using CSM.UiLogic.Workspaces.Playlists;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace CSM.UiLogic.Workspaces
{
    internal class PlaylistsViewModel : BaseWorkspaceViewModel
    {
        #region Private fields

        private BasePlaylistViewModel selectedPlaylist;
        private string playlistPath;
        private BackgroundWorker bgWorker;
        private bool isLoading;
        private int loadProgress;
        private int playlistCount;

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
            }
        }

        /// <summary>
        /// Command used to refresh the workspace data.
        /// </summary>
        public RelayCommand RefreshCommand { get; }

        /// <summary>
        /// Command used to delete the selected custom level.
        /// </summary>
        public RelayCommand DeleteCustomLevelCommand { get; }

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
            DeleteCustomLevelCommand = new RelayCommand(DeletePlaylist, CanDeletePlaylist);
            UserConfigManager.UserConfigChanged += UserConfigManager_UserConfigChanged;
        }

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
        }

        public override void UnloadData()
        {
            base.UnloadData();
            if (bgWorker != null && bgWorker.IsBusy) bgWorker.CancelAsync();
            Playlists.Clear();
            PlaylistPath = null;
        }

        #region Helper methods

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
                            playlists.Add(new PlaylistViewModel(playlist));
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

        private void GetDirectoriesRecursive(PlaylistFolderViewModel folder)
        {
            IEnumerable<string> folderEntries = Directory.EnumerateDirectories(folder.FolderPath);
            foreach (string folderEntry in folderEntries)
            {
                var directory = new DirectoryInfo(folderEntry);
                var playListFolder = new PlaylistFolderViewModel(folderEntry);
                GetDirectoriesRecursive(playListFolder);
                folder.Playlists.Add(playListFolder);
            }

            IEnumerable<string> files = Directory.EnumerateFiles(folder.FolderPath);
            foreach (string file in files)
            {
                if (Path.GetExtension(file) == ".json" || Path.GetExtension(file) == ".bplist")
                {
                    var infoContent = File.ReadAllText(file);
                    Playlist playlist = JsonSerializer.Deserialize<Playlist>(infoContent);
                    if (playlist != null)
                    {
                        folder.Playlists.Add(new PlaylistViewModel(playlist));
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
        }

        private void DeletePlaylist()
        {
            //if (Directory.Exists(SelectedCustomLevel.Path))
            //{
            //    if (MessageBox.Show("Do you want to delete the selected custom level?", "Delete custom level", MessageBoxButton.YesNo) == MessageBoxResult.OK)
            //    {
            //        Directory.Delete(SelectedCustomLevel.Path, true);
            //        CustomLevels.Remove(SelectedCustomLevel);
            //        OnPropertyChanged(nameof(CustomLevelCount));
            //    }
            //}
        }

        public bool CanDeletePlaylist()
        {
            return SelectedPlaylist != null;
        }

        #endregion
    }
}
