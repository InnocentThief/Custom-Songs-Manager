using CSM.DataAccess.Entities.Offline;
using CSM.Framework.Extensions;
using CSM.Services;
using CSM.UiLogic.Workspaces.CustomLevels;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
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
                if (CustomLevels.Count == 0) return "No custom levels loaded";
                if (CustomLevels.Count == 1) return "1 custom level loaded";
                return $"{CustomLevels.Count} custom levels loaded";
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
                loadProgress = (int)(100.0 / 2145 * value);
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
            beatMapService = new BeatMapService();
            itemsObservable = new ObservableCollection<CustomLevelViewModel>();
            itemsCollection = DefaultSort();
            RefreshCommand = new RelayCommand(Refresh);
            DeleteCustomLevelCommand = new RelayCommand(DeleteCustomLevel, CanDeleteCustomLevel);
        }

        /// <summary>
        /// Loads the data in asynchronous fashion.
        /// </summary>
        /// <returns></returns>
        public override void LoadData()
        {
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

        }

        #region Helper methods

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
            IsLoading = true;

            var i = 0;
            var levels = new List<CustomLevel>();

            IEnumerable<string> folderEntries = Directory.EnumerateDirectories("C:\\Users\\dk\\OneDrive\\Madeleine Shared\\BeatSaber\\CustomLevels");
            foreach (string folderEntry in folderEntries)
            {
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
                            customLevel.Path = info;
                        }
                        catch (Exception)
                        {
                            //MessageBox.Show($"Unable to get key for {directory.FullName}. Wrong directory name.");
                        }
                        levels.Add(customLevel);
                    }
                }
                i++;
                bgWorker.ReportProgress(i);
            }
            e.Result = levels;
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bgWorker.Dispose();
            var customLevels = (List<CustomLevel>)e.Result;
            itemsObservable.AddRange(customLevels.Select(cl => new CustomLevelViewModel(cl)));
            IsLoading = false;
            OnPropertyChanged(nameof(CustomLevelCount));
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            LoadProgress = e.ProgressPercentage;
        }

        private void DeleteCustomLevel()
        {
            var directoryPath = Path.GetDirectoryName(SelectedCustomLevel.Path);
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
                CustomLevels.Remove(SelectedCustomLevel);
                OnPropertyChanged(nameof(CustomLevelCount));
            }
        }

        public bool CanDeleteCustomLevel()
        {
            return SelectedCustomLevel != null;
        }

        #endregion
    }
}