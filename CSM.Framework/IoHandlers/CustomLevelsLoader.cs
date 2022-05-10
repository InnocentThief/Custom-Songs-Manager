using CSM.DataAccess.Entities.Offline;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json;

namespace CSM.Framework.IoHandlers
{
    public class CustomLevelsLoader
    {
        private List<CustomLevel> customLevels;
        private BackgroundWorker backgroundWorker;
        private string path;

        public event EventHandler<ProgressChangedEventArgs> ProgressChanged;

        public CustomLevelsLoader()
        {
            backgroundWorker = new BackgroundWorker();
            customLevels = new List<CustomLevel>();
        }

        public List<CustomLevel> LoadCustomLevels(string path)
        {
            this.path = path;

            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += LoadCustomLevels;
            backgroundWorker.ProgressChanged += LoadCustomLevels_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += LoadCustomLevels_RunWorkerCompleted;
            backgroundWorker.RunWorkerAsync();

            return customLevels;
        }

        private void LoadCustomLevels_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            customLevels = (List<CustomLevel>)e.Result;
        }

        private void LoadCustomLevels(object sender, DoWorkEventArgs e)
        {
            var i = 0;
            var levels = new List<CustomLevel>();

            IEnumerable<string> folderEntries = Directory.EnumerateDirectories(path);
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
                        }
                        catch (Exception)
                        {
                            //MessageBox.Show($"Unable to get key for {directory.FullName}. Wrong directory name.");
                        }
                        levels.Add(customLevel);
                    }
                }
                i++;
                backgroundWorker.ReportProgress(i);
            }
            e.Result = levels;
        }

        private void LoadCustomLevels_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressChanged?.Invoke(this, e);
        }
    }
}
