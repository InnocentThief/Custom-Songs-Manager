using CSM.DataAccess.Entities.Online;
using CSM.UiLogic.Properties;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CSM.UiLogic.Workspaces.CustomLevels
{
    /// <summary>
    /// Represents the custom level detail.
    /// </summary>
    public class CustomLevelDetailViewModel
    {
        private readonly BeatMap beatMap;

        #region Public Properties

        public string Id => beatMap.Id;

        public string SongName => beatMap.Metadata.SongName;

        public string SongSubName => beatMap.Metadata.SongSubName;

        public string SongAuthorName => beatMap.Metadata.SongAuthorName;

        public string LevelAuthorName => beatMap.Metadata.LevelAuthorName;

        public DateTime Uploaded => beatMap.Uploaded;

        public decimal Bpm => Math.Round(beatMap.Metadata.Bpm, 0);

        public decimal Duration => beatMap.Metadata.Duration;

        public int Upvotes => beatMap.Stats.Upvotes;

        public int Downvotes => beatMap.Stats.Downvotes;

        public string Score => $"{Math.Round(beatMap.Stats.Score * 100, 0)}%";

        public string Ranked => beatMap.Ranked ? Resources.Yes : Resources.No;

        public string Qualified => beatMap.Qualified ? Resources.Yes : Resources.No;

        public string CoverUrl => beatMap.LatestVersion.CoverUrl;

        public string Tags
        {
            get
            {
                if (beatMap == null) return string.Empty;
                if (beatMap.Tags == null) return string.Empty;
                return String.Join(", ", beatMap.Tags);
            }
        }

        /// <summary>
        /// Gets the description of the beatmap.
        /// </summary>
        public string Description => beatMap.Description;

        /// <summary>
        /// Gets whether a description is available.
        /// </summary>
        public bool HasDescription => !string.IsNullOrWhiteSpace(beatMap.Description);

        /// <summary>
        /// Contains the <see cref="CustomLevelDifficultyViewModel"/> grouped by characteristc.
        /// </summary>
        public List<CustomLevelCharactersisticViewModel> Characteristics { get; }

        /// <summary>
        /// Gets the command used to copy the bsr key.
        /// </summary>
        public RelayCommand CopyBsrKeyCommand { get; }

        #endregion

        /// <summary>
        /// Initializes a new <see cref="CustomLevelDetailViewModel"/>
        /// </summary>
        /// <param name="beatMap">The <see cref="BeatMap"/>.</param>
        public CustomLevelDetailViewModel(BeatMap beatMap)
        {
            this.beatMap = beatMap;
            var characteristics = beatMap.LatestVersion.Difficulties.GroupBy(d => d.Characteristic);
            Characteristics = new List<CustomLevelCharactersisticViewModel>(characteristics.Select(c => new CustomLevelCharactersisticViewModel(c)));
            CopyBsrKeyCommand = new RelayCommand(CopyBsrKey);
        }

        #region Helper methods

        private void CopyBsrKey()
        {
            Clipboard.SetText($"!bsr {Id}");
        }

        #endregion
    }
}