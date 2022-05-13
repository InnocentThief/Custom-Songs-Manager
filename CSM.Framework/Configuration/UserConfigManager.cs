using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace CSM.Framework.Configuration
{
    public class UserConfigManager : IUserConfigManager
    {
        #region Private fields

        private string tempDirectory;
        private string userConfigPath;
        private UserConfig userConfig;

        #endregion

        #region Public Properties

        public string TempDirectory => tempDirectory;

        public UserConfig Config => userConfig;

        #endregion

        public static event EventHandler<UserConfigChangedEventArgs> UserConfigChanged;

        private UserConfigManager()
        {
            LoadOrCreateUserConfig();
        }

        public void SaveUserConfig()
        {
            if (userConfig != null)
            {
                var config = JsonSerializer.Serialize(userConfig);
                File.WriteAllText(userConfigPath, config);
            }
        }

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
                DefaultWorkspace = WorkspaceType.CustomLevels
            };
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