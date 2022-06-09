using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace CSM.Business.TwitchIntegration.TwitchConfiguration
{
    public class TwitchConfigManager
    {
        #region Private fields

        private string tempDirectory;
        private string twitchConfigPath;
        private TwitchConfig twitchConfig;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the temp directory for the Twitch configuration.
        /// </summary>
        public string TempDirectory => tempDirectory;

        /// <summary>
        /// Gets the Twitch configuration.
        /// </summary>
        public TwitchConfig Config => twitchConfig;

        #endregion

        /// <summary>
        /// Initializes a new <see cref="TwitchConfigManager"/>.
        /// </summary>
        private TwitchConfigManager()
        {
            LoadOrCreateTwitchConfig();
        }

        /// <summary>
        /// Saves the Twitch config to the file.
        /// </summary>
        public void SaveTwitchConfig()
        {
            if (twitchConfig != null)
            {
                var config = JsonSerializer.Serialize(twitchConfig);
                File.WriteAllText(twitchConfigPath, config);
            }
        }

        #region Helper methods

        private void LoadOrCreateTwitchConfig()
        {
            tempDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Custom Songs Manager");
            if (!Directory.Exists(tempDirectory)) Directory.CreateDirectory(tempDirectory);

            twitchConfigPath = Path.Combine(tempDirectory, "TwitchConfig.json");
            if (File.Exists(twitchConfigPath))
            {
                var config = File.ReadAllText(twitchConfigPath);
                twitchConfig = JsonSerializer.Deserialize<TwitchConfig>(config);
            }
            else
            {
                twitchConfig = CreateDefaultTwitchConfig(); ;
                SaveTwitchConfig();
            }
        }

        private TwitchConfig CreateDefaultTwitchConfig()
        {
            return new TwitchConfig()
            {
                UserName = String.Empty,
                AccessToken = string.Empty,
                RefreshToken = string.Empty,
                Channels = new List<TwitchChannel>()
            };
        }

        #endregion

        #region Singleton

        private static TwitchConfigManager instance;

        public static TwitchConfigManager Instance
        {
            get
            {
                if (instance == null) instance = new TwitchConfigManager();
                return instance;
            }
        }

        #endregion
    }
}