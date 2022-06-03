using CSM.DataAccess.Entities.Offline;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Diagnostics;
using System.Globalization;

namespace CSM.UiLogic.Workspaces.Tools.CleanupCustomLevels
{
    public class CustomLevelViewModel
    {
        private CustomLevel customLevel;

        #region Public Properties

        public string BsrKey
        {
            get
            {
                int.TryParse(customLevel.BsrKey, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int result);
                if (BsrKeyHex > 0) return customLevel.BsrKey;
                return string.Empty;
            }
        }

        public int BsrKeyHex
        {
            get
            {
                int.TryParse(customLevel.BsrKey, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int result);
                return result;
            }
        }

        public string SongName => customLevel.SongName;

        public string LevelAuthorName => customLevel.LevelAuthorName;

        public string SongAuthorName => customLevel.SongAuthorName;

        public DateTime ChangeDate => customLevel.ChangeDate;

        public string ErrorFound => customLevel.ErrorFound;

        public string Path => customLevel.Path;

        public bool Cleanup { get; set; }

        public string Result { get; set; }

        public RelayCommand OpenInFileExplorerCommand { get; }

        #endregion

        public CustomLevelViewModel(CustomLevel customLevel)
        {
            this.customLevel = customLevel;
            Cleanup = true;
            OpenInFileExplorerCommand = new RelayCommand(OpenInFileExplorer);
        }

        private void OpenInFileExplorer()
        {
            Process.Start(Path);
        }
    }
}