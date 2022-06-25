using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace CSM.Business.TwitchIntegration.TwitchConfiguration
{
    /// <summary>
    /// Handles the Twitch configuration.
    /// </summary>
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
        /// Adds a channel to the configuration.
        /// </summary>
        /// <param name="name">The name of the channel to add.</param>
        public void AddChannel(string name)
        {
            if (!Config.Channels.Any(c => c.Name == name))
            {
                Config.Channels.Add(new TwitchChannel { Name = name });
                SaveTwitchConfig();
            }
        }

        /// <summary>
        /// Removes a channel from the configuration.
        /// </summary>
        /// <param name="name">The name of the channel to remove.</param>
        public void RemoveChannel(string name)
        {
            var existingChannel = Config.Channels.SingleOrDefault(c => c.Name == name);
            if (existingChannel != null)
            {
                Config.Channels.Remove(existingChannel);
                SaveTwitchConfig();
            }
        }

        /// <summary>
        /// Saves the Twitch config to the file.
        /// </summary>
        public void SaveTwitchConfig()
        {
            if (twitchConfig != null)
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var config = JsonSerializer.Serialize(twitchConfig, options);
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