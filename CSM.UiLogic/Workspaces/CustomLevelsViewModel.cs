using CSM.DataAccess.Entities.Offline;
using CSM.Framework;
using CSM.Framework.Configuration.UserConfiguration;
using CSM.Framework.Extensions;
using CSM.Framework.Logging;
using CSM.Services;
using CSM.UiLogic.Properties;
using CSM.UiLogic.Workspaces.CustomLevels;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CSM.UiLogic.Workspaces
{
    /// <summary>
    /// ViewModel for the Custom Levels workspace.
    /// </summary>
    public class CustomLevelsViewModel : BaseWorkspaceViewModel
    {
        #region Private fields

        private ListCollectionView itemsCollection;
        private ObservableCollection<CustomLevelViewModel> itemsObservable;
        private CustomLevelViewModel selectedCustomLevel;
        private CustomLevelDetailViewModel customLevelDetail;
        private BeatMapService beatMapService;
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
        /// Gets whether the Song Detail area on the bottom is visible.
        /// </summary>
        public bool ShowBottomDetail => UserConfigManager.Instance.Config.CustomLevelsSongDetailPosition == SongDetailPosition.Bottom;

        /// <summary>
        /// Gets whether the Song Detail area on the right is visible.
        /// </summary>
        public bool ShowRightDetail => UserConfigManager.Instance.Config.CustomLevelsSongDetailPosition == SongDetailPosition.Right;

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
            beatMapService = new BeatMapService();
            itemsObservable = new ObservableCollection<CustomLevelViewModel>();
            itemsCollection = DefaultSort();
            RefreshCommand = new RelayCommand(Refresh);
            DeleteCustomLevelCommand = new RelayCommand(DeleteCustomLevel, CanDeleteCustomLevel);
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
            if (e.CustomLevelDetailPositionChanged)
            {
                OnPropertyChanged(nameof(ShowBottomDetail));
                OnPropertyChanged(nameof(ShowRightDetail));
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
                                customLevel.ChangeDate = File.GetLastWriteTime(info);
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
                if (MessageBox.Show(Resources.CustomLevels_Delete_Content, Resources.CustomLevels_Delete_Caption, MessageBoxButton.YesNo) == MessageBoxResult.OK)
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

        #endregion
    }
}