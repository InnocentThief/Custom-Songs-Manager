using CSM.DataAccess.Entities.Offline;
using CSM.Framework;
using CSM.Framework.Configuration.UserConfiguration;
using CSM.Framework.Converter;
using CSM.Framework.Extensions;
using CSM.Framework.Logging;
using CSM.Services;
using CSM.UiLogic.Properties;
using CSM.UiLogic.Wizards;
using CSM.UiLogic.Workspaces.CustomLevels;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Data;
using Telerik.Windows.Controls;
using AppCurrent = System.Windows.Application;
using ImageConverter = CSM.Framework.Converter.ImageConverter;

namespace CSM.UiLogic.Workspaces
{
    /// <summary>
    /// ViewModel for the Custom Levels workspace.
    /// </summary>
    public class CustomLevelsViewModel : BaseWorkspaceViewModel
    {
        #region Private fields

        private readonly ListCollectionView itemsCollection;
        private readonly ObservableCollection<CustomLevelViewModel> itemsObservable;
        private CustomLevelViewModel selectedCustomLevel;
        private CustomLevelDetailViewModel customLevelDetail;
        private readonly BeatMapService beatMapService;
        private BackgroundWorker bgWorker;
        private bool isLoading;
        private int loadProgress;
        private string customLevelPath;

        #endregion

        #region Properties

        /// <summary>
        /// Contains all the custom levels sorted by default sort as defined in <see cref="DefaultSort"/>.
        /// </summary>
        public ListCollectionView CustomLevels => itemsCollection;

        /// <summary>
        /// Gets or sets the currently selected custom level.
        /// </summary>
        public CustomLevelViewModel SelectedCustomLevel
        {
            get => selectedCustomLevel;
            set
            {
                if (value == selectedCustomLevel) return;
                selectedCustomLevel = value;
                OnPropertyChanged();
                DeleteCustomLevelCommand.NotifyCanExecuteChanged();
            }
        }

        /// <summary>
        /// Gets the custom level count.
        /// </summary>
        public string CustomLevelCount
        {
            get
            {
                if (CustomLevels.Count == 0) return Resources.CustomLevels_NoCustomLevelsLoaded;
                if (CustomLevels.Count == 1) return Resources.CustomLevels_OneCustomLevelLoaded;
                return string.Format(Resources.CustomLevels_MultipleCustomLevelsLoaded, CustomLevels.Count);
            }
        }

        /// <summary>
        /// Gets or sets the viewmodel for the detail area.
        /// </summary>
        public CustomLevelDetailViewModel CustomLevelDetail
        {
            get => customLevelDetail;
            set
            {
                if (value == customLevelDetail) return;
                customLevelDetail = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasCustomLevelDetail));
            }
        }

        /// <summary>
        /// Gets whether a custom level detail is available.
        /// </summary>
        public bool HasCustomLevelDetail
        {
            get => customLevelDetail != null;
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
        /// Command used to open the custom levels path in file explorer.
        /// </summary>
        public RelayCommand OpenInFileExplorerCommand { get; }

        /// <summary>
        /// Command used to save all custom levels into a playlist.
        /// </summary>
        public RelayCommand SaveCustomLevelsToPlaylistCommand { get; }

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
                loadProgress = (int)(100.0 / 2145 * value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the custom level path.
        /// </summary>
        public string CustomLevelPath
        {
            get => customLevelPath;
            set
            {
                if (customLevelPath == value) return;
                customLevelPath = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the workspace type.
        /// </summary>
        public override WorkspaceType WorkspaceType => WorkspaceType.CustomLevels;

        #endregion

        /// <summary>
        /// Initializes a new <see cref="CustomLevelsViewModel"/>.
        /// </summary>
        public CustomLevelsViewModel()
        {
            CustomLevelPath = UserConfigManager.Instance.Config.CustomLevelPaths.First().Path;
            beatMapService = new BeatMapService("maps/id");
            itemsObservable = new ObservableCollection<CustomLevelViewModel>();
            itemsCollection = DefaultSort();
            RefreshCommand = new RelayCommand(Refresh);
            DeleteCustomLevelCommand = new RelayCommand(DeleteCustomLevel, CanDeleteCustomLevel);
            OpenInFileExplorerCommand = new RelayCommand(OpenInFileExplorer);
            SaveCustomLevelsToPlaylistCommand = new RelayCommand(SaveToPlaylist);
            UserConfigManager.UserConfigChanged += UserConfigManager_UserConfigChanged;
        }

        /// <summary>
        /// Used to load the workspace data.
        /// </summary>
        public override void LoadData()
        {
            base.LoadData();
            CustomLevelPath = UserConfigManager.Instance.Config.CustomLevelPaths.First().Path;

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

        /// <summary>
        /// Unloads the data.
        /// </summary>
        public override void UnloadData()
        {
            base.UnloadData();
            if (bgWorker != null && bgWorker.IsBusy) bgWorker.CancelAsync();
            itemsObservable.Clear();
            CustomLevelDetail = null;
        }

        /// <summary>
        /// Loads the custom level data from BeatSaver.
        /// </summary>
        /// <param name="key">The key of the beat map.</param>
        /// <returns>An awaitable task that yields no result.</returns>
        public async Task GetBeatSaverBeatMapDataAsync(string key)
        {
            var beatmap = await beatMapService.GetBeatMapDataAsync(key);
            CustomLevelDetail = beatmap == null ? null : new CustomLevelDetailViewModel(beatmap);
        }

        #region Helper methods

        private void UserConfigManager_UserConfigChanged(object sender, UserConfigChangedEventArgs e)
        {
            if (e.CustomLevelsPathChanged)
            {
                CustomLevelPath = UserConfigManager.Instance.Config.CustomLevelPaths.First().Path;
                if (IsActive) Refresh();
            }
        }

        private ListCollectionView DefaultSort()
        {
            var collection = new ListCollectionView(itemsObservable);
            collection.SortDescriptions.Add(new SortDescription("ChangeDate", ListSortDirection.Descending));
            return collection;
        }

        private void Refresh()
        {
            itemsObservable.Clear();
            LoadData();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                IsLoading = true;

                var i = 0;
                var levels = new List<CustomLevel>();

                if (!Directory.Exists(CustomLevelPath)) return;

                IEnumerable<string> folderEntries = Directory.EnumerateDirectories(CustomLevelPath);
                foreach (string folderEntry in folderEntries)
                {
                    if (bgWorker.CancellationPending) return;
                    var info = Path.Combine(folderEntry, "Info.dat");
                    if (File.Exists(info))
                    {
                        var infoContent = File.ReadAllText(info);
                        CustomLevel customLevel = JsonSerializer.Deserialize<CustomLevel>(infoContent);
                        if (customLevel != null)
                        {
                            var directory = new DirectoryInfo(folderEntry);
                            try
                            {
                                customLevel.BsrKey = directory.Name.Substring(0, directory.Name.IndexOf(" "));
                                var bsrKeyHex = int.Parse(customLevel.BsrKey, System.Globalization.NumberStyles.HexNumber);
                                customLevel.ChangeDate = Directory.GetLastWriteTime(folderEntry);
                                customLevel.Path = folderEntry;
                            }
                            catch (Exception)
                            {
                                LoggerProvider.Logger.Info<CustomLevelsViewModel>($"Unable to get key for {directory.FullName}. Wrong directory name");
                            }
                            levels.Add(customLevel);
                        }
                    }
                    i++;
                    bgWorker.ReportProgress(i);
                }
                e.Result = levels;
            }
            catch (Exception ex)
            {
                LoggerProvider.Logger.Error<CustomLevelsViewModel>($"Unable to load custom levels: {ex}");
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var customLevels = (List<CustomLevel>)e.Result;
            if (customLevels != null)
            {
                itemsObservable.AddRange(customLevels.Select(cl => new CustomLevelViewModel(cl)));
            }
            IsLoading = false;
            OnPropertyChanged(nameof(CustomLevelCount));

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

        private void DeleteCustomLevel()
        {
            if (Directory.Exists(SelectedCustomLevel.Path))
            {
                var messageBoxViewModel = new MessageBoxViewModel(Resources.CustomLevels_Delete_Caption, MessageBoxButtonColor.Attention, Resources.Cancel, MessageBoxButtonColor.Default)
                {
                    Title = Resources.CustomLevels_Delete_Caption,
                    Message = Resources.CustomLevels_Delete_Content,
                    MessageBoxType = DataAccess.Entities.Types.MessageBoxTypes.Question
                };
                MessageBoxController.Instance().ShowMessageBox(messageBoxViewModel);
                if (messageBoxViewModel.Continue)
                {
                    Directory.Delete(SelectedCustomLevel.Path, true);
                    CustomLevels.Remove(SelectedCustomLevel);
                    OnPropertyChanged(nameof(CustomLevelCount));
                }
            }
        }

        public bool CanDeleteCustomLevel()
        {
            return SelectedCustomLevel != null;
        }

        private void OpenInFileExplorer()
        {
            Process.Start(UserConfigManager.Instance.Config.CustomLevelPaths.First().Path);
        }

        private void SaveToPlaylist()
        {
            var defaultImageLocation = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Images\\CSM_Logo_400px.png");

            var playlist = new Playlist
            {
                Path = String.Empty,
                PlaylistAuthor = "Custom Songs Manager",
                PlaylistDescription = "Contains all custom songs at the backup date",
                PlaylistTitle = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Hour}-{DateTime.Now.Minute}-{DateTime.Now.Second}-SavedCustomLevelReferences",
                CustomData = null,
                Songs = new List<PlaylistSong>(),
                Image = $"base64,{ImageConverter.StringFromBitmap(defaultImageLocation)}"
            };

            foreach (var customLevel in itemsObservable)
            {
                playlist.Songs.Add(new PlaylistSong
                {
                    Key = customLevel.BsrKey,
                    LevelAuthorName = customLevel.LevelAuthorName,
                    SongName = customLevel.SongName,
                }); ;
            }

            // Save to file
            var playlistPath = UserConfigManager.Instance.Config.PlaylistPaths.First().Path;
            if (!Directory.Exists(playlistPath)) playlistPath = "C:\\";

            RadSaveFileDialog saveFileDialog = new RadSaveFileDialog
            {
                Owner = AppCurrent.Current.MainWindow,
                InitialDirectory = playlistPath,
                FileName = $"{playlist.PlaylistTitle}.json"
            };
            saveFileDialog.ShowDialog();
            if (saveFileDialog.DialogResult==true)
            {
                playlistPath = saveFileDialog.FileName;
                var options = new JsonSerializerOptions { WriteIndented = true };
                var content = JsonSerializer.Serialize(playlist, options);
                File.WriteAllText(playlistPath, content);
            }
        }

        #endregion
    }
}