using CSM.DataAccess.Entities.Offline;
using CSM.Framework.Configuration.UserConfiguration;
using CSM.Framework.Extensions;
using CSM.Framework.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.Tools.CleanupCustomLevels
{
    public class StepDirectoryNamesViewModel : StepBaseViewModel
    {
        #region Private fields

        private BackgroundWorker bgWorker;
        private bool isLoading;
        private int loadProgress;

        #endregion

        #region Public Properties

        public ObservableCollection<CustomLevelViewModel> CustomLevels { get; }

        public StepDirectoryNamesViewModel() : base("Directory Names", "Cleanup wrong directory names")
        {
            CustomLevels = new ObservableCollection<CustomLevelViewModel>();
        }

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

        #endregion

        public override async Task LoadDataAsync()
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

            await Task.CompletedTask;
        }

        #region Helper methods

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                IsLoading = true;

                var i = 0;
                var levels = new List<CustomLevel>();
                var customLevelsPath = UserConfigManager.Instance.Config.CustomLevelPaths.First().Path;

                if (!Directory.Exists(customLevelsPath)) return;

                IEnumerable<string> folderEntries = Directory.EnumerateDirectories(customLevelsPath);
                foreach (string folderEntry in folderEntries)
                {
                    if (bgWorker.CancellationPending) return;

                    var directory = new DirectoryInfo(folderEntry);
                    var bsrKey = directory.Name.Substring(0, directory.Name.IndexOf(" "));
                    try
                    {
                        var bsrKeyHex = int.Parse(bsrKey, System.Globalization.NumberStyles.HexNumber);
                    }
                    catch (Exception)
                    {
                        var info = Path.Combine(folderEntry, "Info.dat");
                        if (File.Exists(info))
                        {
                            var infoContent = File.ReadAllText(info);
                            CustomLevel customLevel = JsonSerializer.Deserialize<CustomLevel>(infoContent);
                            if (customLevel != null)
                            {
                                customLevel.ChangeDate = Directory.GetLastWriteTime(folderEntry);
                                customLevel.Path = folderEntry;
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
                CustomLevels.AddRange(customLevels.Select(cl => new CustomLevelViewModel(cl)));
            }
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

        #endregion
    }
}
