using CSM.Business.Interfaces;
using CSM.DataAccess.UserConfiguration;
using CSM.Framework.Types;
using System.Text.Json;

namespace CSM.Business.Core
{
    internal sealed class UserConfigDomain : IUserConfigDomain
    {
        #region Private fields

        private string? tempDirectory;
        private string? userConfigPath;
        private UserConfig? userConfig;
        private readonly JsonSerializerOptions jsonSerializerOptions = new() { WriteIndented = true };

        #endregion

        public string? TempDirectory => tempDirectory;

        public UserConfig? Config => userConfig;

        public void LoadOrCreateUserConfig()
        {
            tempDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Custom Songs Manager");
            if (!Directory.Exists(tempDirectory)) Directory.CreateDirectory(tempDirectory);

            userConfigPath = Path.Combine(tempDirectory, "UserConfig.json");
            if (File.Exists(userConfigPath))
            {
                var config = File.ReadAllText(userConfigPath);
                if (config != null)
                {
                    userConfig = JsonSerializer.Deserialize<UserConfig>(config);
                    CheckForMissingSettingValues();
                }
                else
                {
                    userConfig = CreateDefaultUserConfig();
                    SaveUserConfig();
                }
            }
            else
            {
                userConfig = CreateDefaultUserConfig();
                SaveUserConfig();
            }
        }

        public void SaveUserConfig()
        {
            if (userConfig != null && !string.IsNullOrWhiteSpace(userConfigPath))
            {
                var config = JsonSerializer.Serialize(userConfig, jsonSerializerOptions);
                File.WriteAllText(userConfigPath, config);
            }
        }

        #region Helper methods

        private static UserConfig CreateDefaultUserConfig()
        {
            return new UserConfig()
            {
                BeatSaberInstallPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Beat Saber",
                BeatSaverAPIEndpoint = "https://api.beatsaver.com/",
                CustomLevelPaths =
                [
                    new CustomLevelPath()
                    {
                        Default = true,
                        Path = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Beat Saber\\Beat Saber_Data\\CustomLevels",
                        Name = "Default Custom Levels Path"
                    }
                ],
                PlaylistPaths =
                [
                    new PlaylistPath()
                    {
                        Default = true,
                        Path = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Beat Saber\\Playlists",
                        Name = "Default Playlists Path"
                    }
                ],
                DefaultWorkspace = NavigationType.CustomLevels,
                CustomLevelsSongDetailPosition = SongDetailPosition.Right,
                RemoveReceivedSongAfterAddingToPlaylist = false,
            };
        }

        private void CheckForMissingSettingValues()
        {
            if (Config != null && string.IsNullOrWhiteSpace(Config.BeatSaverAPIEndpoint))
            {
                Config.BeatSaverAPIEndpoint = "https://api.beatsaver.com/";
            }
            SaveUserConfig();
        }

        #endregion
    }
}
