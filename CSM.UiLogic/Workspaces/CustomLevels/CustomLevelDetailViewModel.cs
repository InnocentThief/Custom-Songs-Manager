using CSM.DataAccess.Entities.Online;
using CSM.UiLogic.Properties;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CSM.UiLogic.Workspaces.CustomLevels
{
    public class CustomLevelDetailViewModel
    {
        private BeatMap beatMap;

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

        public string CoverUrl => beatMap.Versions.First().CoverUrl;

        public string Tags
        {
            get
            {
                if (beatMap == null) return string.Empty;
                if (beatMap.Tags == null) return string.Empty;
                return String.Join(", ", beatMap.Tags);
            }
        }

        public string Description => beatMap.Description;

        public List<CustomLevelCharactersisticViewModel> Characteristics { get; }

        public RelayCommand CopyBsrKeyCommand { get; }

        #endregion

        public CustomLevelDetailViewModel(BeatMap beatMap)
        {
            this.beatMap = beatMap;
            var difficulties = beatMap.Versions.SelectMany(v => v.Difficulties);
            var characteristics = difficulties.GroupBy(d => d.Characteristic);
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