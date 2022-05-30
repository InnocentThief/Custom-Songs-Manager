using CSM.DataAccess.Entities.Offline;
using CSM.Framework.Configuration.UserConfiguration;
using CSM.Framework.Extensions;
using CSM.Framework.Logging;
using CSM.Services;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
        private BeatMapService beatMapService;

        #endregion

        #region Public Properties

        public ObservableCollection<CustomLevelViewModel> CustomLevels { get; }

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

        public bool Processed { get; set; }

        public AsyncRelayCommand StartCleanupCommand { get; }

        public RelayCommand ProgressStepCommand { get; }

        #endregion

        public StepDirectoryNamesViewModel() : base("Directory Names", "Cleanup wrong directory names")
        {
            CustomLevels = new ObservableCollection<CustomLevelViewModel>();

            StartCleanupCommand = new AsyncRelayCommand(StartCleanupAsync);
            ProgressStepCommand = new RelayCommand(ProgressStep, CanProgressStep);

            beatMapService = new BeatMapService(String.Empty);
        }

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

                    var directoryInfo = new DirectoryInfo(folderEntry);
                    var errorText = string.Empty;
                    var bsrKey = directoryInfo.Name.Substring(0, directoryInfo.Name.IndexOf(" "));
                    if (HasMissingOrWrongKey(directoryInfo))
                    {
                        errorText = "Missing or wrong BSR key";
                        bsrKey = string.Empty;
                    }
                    else if (HasMissingFiles(directoryInfo))
                    {
                        if (string.IsNullOrWhiteSpace(errorText)) errorText = "Missing files";
                        else errorText += " + more";
                    }
                    else if (HasSubDirectery(directoryInfo))
                    {
                        if (string.IsNullOrWhiteSpace(errorText)) errorText = "Contains sub directory";
                        else if (!errorText.Contains("more")) errorText += " + more";
                    }

                    if (!string.IsNullOrWhiteSpace(errorText))
                    {
                        var info = Path.Combine(folderEntry, "Info.dat");
                        if (File.Exists(info))
                        {
                            var infoContent = File.ReadAllText(info);
                            CustomLevel customLevel = JsonSerializer.Deserialize<CustomLevel>(infoContent);
                            if (customLevel != null)
                            {
                                customLevel.BsrKey = bsrKey;
                                customLevel.ErrorFound = errorText;
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

        private bool HasMissingOrWrongKey(DirectoryInfo directoryInfo)
        {
            var bsrKey = directoryInfo.Name.Substring(0, directoryInfo.Name.IndexOf(" "));
            if (string.IsNullOrWhiteSpace(bsrKey)) return true;
            int.TryParse(bsrKey, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int result);
            if (result == 0) return true;
            return false;
        }

        private bool HasMissingFiles(DirectoryInfo directoryInfo)
        {
            var directoryPath = directoryInfo.FullName;
            if (!Directory.EnumerateFiles(directoryPath, "*.*").Any(f => f.EndsWith("jpg") || f.EndsWith("jpeg") || f.EndsWith("png"))) return true;
            if (!File.Exists(Path.Combine(directoryPath, "Info.dat"))) return true;
            if (!Directory.EnumerateFiles(directoryPath, "*.egg").Any()) return true;
            return !Directory.EnumerateFiles(directoryPath, "*.dat", SearchOption.TopDirectoryOnly)
                .Any(f => f.Contains("Easy") || f.Contains("Normal") || f.Contains("Hard") || f.Contains("Expert"));
        }

        private bool HasSubDirectery(DirectoryInfo directoryInfo)
        {
            return Directory.EnumerateDirectories(directoryInfo.FullName).Any();
        }

        private async Task StartCleanupAsync()
        {
            foreach (var customLevel in CustomLevels)
            {

                //var infoPath = Path.Combine(customLevel.Path, "Info.dat");
             

                //var infoContent = File.ReadAllText(infoPath);
                //var hasher = SHA1.Create();
                //hasher.ComputeHash(infoContent);

                //var cl = JsonSerializer.Deserialize<CustomLevel>(infoContent);

                //foreach (var difficultySet in cl.DifficultySets)
                //{
                //    foreach (var difficulty in difficultySet.Difficulties)
                //    {
                //        var diffFile = Path.Combine(customLevel.Path, difficulty.BeatmapFilename);
                //        var diffFileContent = File.ReadAllText(diffFile);
                //    }
                //}










                var user = await beatMapService.GetUserByNameAsync(customLevel.LevelAuthorName);
                if (user != null)
                {
                    var beatmaps = await beatMapService.GetBeatMapsByUserIdAsync(user.Id);

                }
            }












            Processed = true;
            ProgressStepCommand.NotifyCanExecuteChanged();
        }

        public void ProgressStep()
        {
            Progress();
        }

        public bool CanProgressStep()
        {
            return Processed;
        }

        #endregion
    }
}