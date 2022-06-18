using CSM.DataAccess.Entities.Offline;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace CSM.Business.TwitchIntegration
{
    /// <summary>
    /// Handles received beatmaps.
    /// </summary>
    public class ReceivedBeatmapsManager
    {
        #region Private Fields

        private string tempDirectory;
        private string receivedBeatmapsPath;

        #endregion

        /// <summary>
        /// Contains the received beatmaps.
        /// </summary>
        public ReceivedBeatmaps ReceivedBeatmaps;

        /// <summary>
        /// Initializes a new <see cref="ReceivedBeatmapsManager"/>.
        /// </summary>
        private ReceivedBeatmapsManager()
        {
            LoadOrCreateReceivedBeatmaps();
        }

        /// <summary>
        /// Adds a beatmap to the configuration.
        /// </summary>
        /// <param name="receivedBeatmap">The beatmap to add.</param>
        public void AddBeatmap(ReceivedBeatmap receivedBeatmap)
        {
            if (!ReceivedBeatmaps.Beatmaps.Any(bm => bm.Key == receivedBeatmap.Key))
            {
                ReceivedBeatmaps.Beatmaps.Add(receivedBeatmap);
                Save();
            }
        }

        /// <summary>
        /// Removes a beatmap from the configuration.
        /// </summary>
        /// <param name="beatmapsToRemove">The beatmap to remove.</param>
        public void RemoveBeatmaps(List<ReceivedBeatmap> beatmapsToRemove)
        {
            foreach (var beatmapToRemove in beatmapsToRemove)
            {
                var beatmap = ReceivedBeatmaps.Beatmaps.SingleOrDefault(bm => bm.Key == beatmapToRemove.Key);
                if (beatmap != null)
                {
                    ReceivedBeatmaps.Beatmaps.Remove(beatmap);
                }
            }
            Save();
        }

        #region Helper methods

        private void LoadOrCreateReceivedBeatmaps()
        {
            tempDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Custom Songs Manager");
            if (!Directory.Exists(tempDirectory)) Directory.CreateDirectory(tempDirectory);

            receivedBeatmapsPath = Path.Combine(tempDirectory, "ReceivedBeatmaps.json");
            if (File.Exists(receivedBeatmapsPath))
            {
                var config = File.ReadAllText(receivedBeatmapsPath);
                ReceivedBeatmaps = JsonSerializer.Deserialize<ReceivedBeatmaps>(config);
            }
            else
            {
                ReceivedBeatmaps = CreateDefaultReceivedBeatmaps(); ;
                Save();
            }
        }

        private ReceivedBeatmaps CreateDefaultReceivedBeatmaps()
        {
            return new ReceivedBeatmaps
            {
                Beatmaps = new List<ReceivedBeatmap>()
            };
        }

        private void Save()
        {
            if (ReceivedBeatmaps != null)
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var config = JsonSerializer.Serialize(ReceivedBeatmaps, options);
                File.WriteAllText(receivedBeatmapsPath, config);
            }
        }

        #endregion


        #region Singleton

        private static ReceivedBeatmapsManager instance;

        public static ReceivedBeatmapsManager Instance
        {
            get
            {
                if (instance == null) instance = new ReceivedBeatmapsManager();
                return instance;
            }
        }

        #endregion
    }
}