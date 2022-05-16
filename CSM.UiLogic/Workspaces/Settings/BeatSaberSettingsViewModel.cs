using CSM.Framework.Configuration.UserConfiguration;
using CSM.Framework.Logging;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace CSM.UiLogic.Workspaces.Settings
{
    /// <summary>
    /// Contains the settings related to Beat Saber.
    /// </summary>
    public class BeatSaberSettingsViewModel : ObservableObject
    {
        #region Private fields

        private string beatSaberInstallPath;
        private string customLevelsPath;
        private string playlistsPath;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Beat Saber installation path.
        /// </summary>
        public string BeatSaberInstallPath
        {
            get => beatSaberInstallPath;
            set
            {
                if (value == beatSaberInstallPath) return;
                if (ValidatePath(value))
                {
                    beatSaberInstallPath = value;
                    UserConfigManager.Instance.Config.BeatSaberInstallPath = beatSaberInstallPath;
                    UserConfigManager.Instance.SaveUserConfig();
                }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Beat Saver custom level path.
        /// </summary>
        public string CustomLevelsPath
        {
            get => customLevelsPath;
            set
            {
                if (value == customLevelsPath) return;
                if (ValidatePath(value))
                {
                    customLevelsPath = value;
                    UserConfigManager.Instance.Config.CustomLevelPaths.First().Path = customLevelsPath;
                    UserConfigManager.Instance.Changed(new UserConfigChangedEventArgs() { CustomLevelsPathChanged = true });
                    UserConfigManager.Instance.SaveUserConfig();
                }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Beat Saber playlist path.
        /// </summary>
        public string PlaylistsPath
        {
            get => playlistsPath;
            set
            {
                if (value == playlistsPath) return;
                if (ValidatePath(value))
                {
                    playlistsPath = value;
                    UserConfigManager.Instance.Config.PlaylistPaths.First().Path = playlistsPath;
                    UserConfigManager.Instance.SaveUserConfig();
                }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Command used to show the file dialog to select a directory.
        /// </summary>
        public RelayCommand<object> SelectDirectoryCommand { get; }

        #endregion

        /// <summary>
        /// Initializes a new <see cref="BeatSaberSettingsViewModel"/>.
        /// </summary>
        public BeatSaberSettingsViewModel()
        {
            beatSaberInstallPath = UserConfigManager.Instance.Config.BeatSaberInstallPath;
            customLevelsPath = UserConfigManager.Instance.Config.CustomLevelPaths.First().Path;
            playlistsPath = UserConfigManager.Instance.Config.PlaylistPaths.First().Path;

            SelectDirectoryCommand = new RelayCommand<object>(SelectDirectory);
        }

        #region Helper methods

        private void SelectDirectory(object type)
        {
            try
            {
                var inputPath = string.Empty;

                switch (type)
                {
                    case "BeatSaberInstallPath":
                        inputPath = BeatSaberInstallPath;
                        break;
                    case "CustomLevelsPath":
                        inputPath = CustomLevelsPath;
                        break;
                    case "PlaylistsPath":
                        inputPath = PlaylistsPath;
                        break;
                    default:
                        inputPath = BeatSaberInstallPath;
                        break;
                }

                var dlg = new FolderPicker();
                if (!Directory.Exists(inputPath)) inputPath = "c:\\";
                dlg.InputPath = inputPath;
                if (dlg.ShowDialog() == true)
                {
                    if (ValidatePath(dlg.ResultPath))
                    {
                        switch (type)
                        {
                            case "BeatSaberInstallPath":
                                BeatSaberInstallPath = dlg.ResultPath;
                                break;
                            case "CustomLevelsPath":
                                CustomLevelsPath = dlg.ResultPath;
                                break;
                            case "PlaylistsPath":
                                PlaylistsPath = dlg.ResultPath;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Unable to open file dialog");
                LoggerProvider.Logger.Error<BeatSaberSettingsViewModel>($"Unable to open file dialog: {ex}");
            }
        }

        private bool ValidatePath(string path)
        {
            var ret = Directory.Exists(path);
            if (!ret)
            {
                MessageBox.Show("Path does not exist. Please enter a new path", "Wrong path");
            }
            return ret;
        }

        #endregion
    }
}