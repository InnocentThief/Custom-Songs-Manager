﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CSM.DataAccess.Entities.Offline
{
    public class CustomLevel
    {
        [JsonIgnore]
        public string BsrKey { get; set; }

        [JsonPropertyName("_version")]
        public string Version { get; set; }

        [JsonPropertyName("_songName")]
        public string SongName { get; set; }

        [JsonPropertyName("_songSubName")]
        public string SongSubName { get; set; }

        [JsonPropertyName("_songAuthorName")]
        public string SongAuthorName { get; set; }

        [JsonPropertyName("_levelAuthorName")]
        public string LevelAuthorName { get; set; }

        [JsonPropertyName("_beatsPerMinute")]
        public decimal BeatsPerMinute { get; set; }

        [JsonPropertyName("_shuffle")]
        public decimal Shuffle { get; set; }

        [JsonPropertyName("_shufflePeriod")]
        public decimal ShufflePeriod { get; set; }

        [JsonPropertyName("_previewStartTime")]
        public decimal PreviewStartTime { get; set; }

        [JsonPropertyName("_previewDuration")]
        public decimal PreviewDuration { get; set; }

        [JsonPropertyName("_songFilename")]
        public string SongFilename { get; set; }

        [JsonPropertyName("_coverImageFilename")]
        public string CoverImageFilename { get; set; }

        [JsonPropertyName("_environmentName")]
        public string EnvironmentName { get; set; }

        [JsonPropertyName("_songTimeOffset")]
        public decimal SongTimeOffset { get; set; }

        [JsonIgnore]
        public DateTime ChangeDate { get; set; }

        [JsonPropertyName("_customData")]
        public CustomLevelCustomData CustomData { get; set; }

        [JsonPropertyName("_difficultyBeatmapSets")]
        public List<BeatMapDifficultySet> DifficultiySets { get; set; }
    }
}
