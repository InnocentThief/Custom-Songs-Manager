using CSM.Business.Interfaces;
using CSM.DataAccess.UserConfiguration;
using CSM.Framework.Types;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CSM.Business.Core
{
    internal sealed class UserConfigDomain(ILogger<UserConfigDomain> logger) : IUserConfigDomain
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
                    try
                    {
                        userConfig = JsonSerializer.Deserialize<UserConfig>(config);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error deserializing UserConfig from {userConfigPath}", userConfigPath);
                        throw;
                    }

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
                DefaultWorkspace = NavigationType.CustomLevels,
                BeatSaberInstallPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Beat Saber",
                BeatSaverAPIEndpoint = "https://api.beatsaver.com/",
                CustomLevelsConfig = new CustomLevelsConfig()
                {
                    Available = true,
                    CustomLevelPath = new CustomLevelPath()
                    {
                        Default = true,
                        Path = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Beat Saber\\Beat Saber_Data\\CustomLevels",
                        Name = "Default Custom Levels Path"
                    },
                    SongDetailPosition = SongDetailPosition.Right,
                },
                PlaylistsConfig = new PlaylistsConfig()
                {
                    Available = true,
                    PlaylistPath = new PlaylistPath()
                    {
                        Default = true,
                        Path = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Beat Saber\\Playlists",
                        Name = "Default Playlists Path"
                    },
                    SourceAvailability = PlaylistsSourceAvailability.CustomLevels | PlaylistsSourceAvailability.Playlists | PlaylistsSourceAvailability.BeatSaberFavourites | PlaylistsSourceAvailability.SongSearch | PlaylistsSourceAvailability.SongSuggest,
                    DefaultSource = PlaylistsSourceAvailability.CustomLevels,
                },
                TwitchConfig = new TwitchConfig()
                {
                    Available = true,
                    RemoveReceivedSongAfterAddingToPlaylist = false,
                },
                ScoreSaberConfig = new ScoreSaberConfig()
                {
                    Available = true,
                },
                BeatLeaderConfig = new BeatLeaderConfig()
                {
                    Available = true,
                },
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
