using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace CSM.Framework.Configuration.UserConfiguration
{
    /// <summary>
    /// Handles the user configuration.
    /// </summary>
    public class UserConfigManager : IUserConfigManager
    {
        #region Private fields

        private string tempDirectory;
        private string userConfigPath;
        private UserConfig userConfig;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the temp directory for the user.
        /// </summary>
        public string TempDirectory => tempDirectory;

        /// <summary>
        /// Gets the config for the user.
        /// </summary>
        public UserConfig Config => userConfig;

        #endregion

        /// <summary>
        /// Occurs on settings change.
        /// </summary>
        public static event EventHandler<UserConfigChangedEventArgs> UserConfigChanged;

        /// <summary>
        /// Initializes a new <see cref="UserConfigManager"/>.
        /// </summary>
        private UserConfigManager()
        {
            LoadOrCreateUserConfig();
        }

        /// <summary>
        /// Saves the user config to the file.
        /// </summary>
        public void SaveUserConfig()
        {
            if (userConfig != null)
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var config = JsonSerializer.Serialize(userConfig, options);
                File.WriteAllText(userConfigPath, config);
            }
        }

        /// <summary>
        /// Invokes the user config changed event.
        /// </summary>
        /// <param name="userConfigChangedEventArgs">EventArgs containing the information on what setting has changed.</param>
        public void Changed(UserConfigChangedEventArgs userConfigChangedEventArgs)
        {
            UserConfigChanged?.Invoke(this, userConfigChangedEventArgs);
        }

        #region Helper methods

        private void LoadOrCreateUserConfig()
        {
            tempDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Custom Songs Manager");
            if (!Directory.Exists(tempDirectory)) Directory.CreateDirectory(tempDirectory);

            userConfigPath = Path.Combine(tempDirectory, "UserConfig.json");
            if (File.Exists(userConfigPath))
            {
                var config = File.ReadAllText(userConfigPath);
                userConfig = JsonSerializer.Deserialize<UserConfig>(config);
                CheckForMissingSettingValues();
            }
            else
            {
                userConfig = CreateDefaultUserConfig();
                SaveUserConfig();
            }
        }

        private UserConfig CreateDefaultUserConfig()
        {
            return new UserConfig()
            {
                BeatSaberInstallPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Beat Saber",
                BeatSaverAPIEndpoint = "https://api.beatsaver.com/",
                CustomLevelPaths = new List<CustomLevelPath>
                {
                    new CustomLevelPath()
                    {
                        Default = true,
                        Path = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Beat Saber\\Beat Saber_Data\\CustomLevels",
                        Name = Properties.Resources.DefaultCustomLevelsPath
                    }
                },
                PlaylistPaths = new List<PlaylistPath>
                {
                    new PlaylistPath()
                    {
                        Default = true,
                        Path = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Beat Saber\\Playlists",
                        Name = Properties.Resources.DefaultPlaylistsPath
                    }
                },
                DefaultWorkspace = WorkspaceType.CustomLevels,
                CustomLevelsSongDetailPosition = SongDetailPosition.Right,
                ScoreSaberAnalysisMode = ScoreSaberAnalysisMode.Single,
                RemoveReceivedSongAfterAddingToPlaylist = false,
            };
        }

        private void CheckForMissingSettingValues()
        {
            if (string.IsNullOrWhiteSpace(Config.BeatSaverAPIEndpoint))
            {
                Config.BeatSaverAPIEndpoint = "https://api.beatsaver.com/";
            }
            SaveUserConfig();
        }

        #endregion

        #region Singleton

        private static UserConfigManager instance;

        public static UserConfigManager Instance
        {
            get
            {
                if (instance == null) instance = new UserConfigManager();
                return instance; ;
            }
        }

        #endregion
    }
}