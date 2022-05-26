using CSM.Framework.Configuration.UserConfiguration;
using CSM.Framework.Logging;
using CSM.UiLogic.Properties;
using CSM.UiLogic.Wizards;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.IO;
using System.Linq;

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
                var messageBoxViewModel = new MessageBoxViewModel(Resources.OK, MessageBoxButtonColor.Default, string.Empty, MessageBoxButtonColor.Default)
                {
                    Title = Resources.Settings_BeatSaber_FileDialog,
                    Message = Resources.Settings_BeatSaber_FileDialog_Error,
                    MessageBoxType = DataAccess.Entities.Types.MessageBoxTypes.Warning
                };
                MessageBoxController.Instance().ShowMessageBox(messageBoxViewModel);
                LoggerProvider.Logger.Error<BeatSaberSettingsViewModel>($"Unable to open file dialog: {ex}");
            }
        }

        private bool ValidatePath(string path)
        {
            var ret = Directory.Exists(path);
            if (!ret)
            {
                var messageBoxViewModel = new MessageBoxViewModel(Resources.OK, MessageBoxButtonColor.Default, string.Empty, MessageBoxButtonColor.Default)
                {
                    Title = Resources.Settings_BeatSaber_ValidatePath_Caption,
                    Message = Resources.Settings_BeatSaber_ValidatePath_Content,
                    MessageBoxType = DataAccess.Entities.Types.MessageBoxTypes.Warning
                };
                MessageBoxController.Instance().ShowMessageBox(messageBoxViewModel);
            }
            return ret;
        }

        #endregion
    }
}