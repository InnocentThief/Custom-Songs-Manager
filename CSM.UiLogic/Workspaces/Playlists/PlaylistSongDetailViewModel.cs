using CSM.DataAccess.Entities.Offline;
using CSM.DataAccess.Entities.Online;
using CSM.UiLogic.Properties;
using CSM.UiLogic.Wizards;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;

namespace CSM.UiLogic.Workspaces.Playlists
{
    /// <summary>
    /// Represents the playlist sond detail.
    /// </summary>
    public class PlaylistSongDetailViewModel
    {
        private readonly BeatMap beatMap;

        #region Public Properties

        public string SongName => beatMap.Metadata.SongName;

        public string SongSubName => beatMap.Metadata.SongSubName;

        public string SongAuthorName => beatMap.Metadata.SongAuthorName;

        public string LevelAuthorName => beatMap.Metadata.LevelAuthorName;

        public string Score => $"{Math.Round(beatMap.Stats.Score * 100, 0)}%";

        public string Ranked => beatMap.Ranked ? Resources.Yes : Resources.No;

        public string CoverUrl => beatMap.LatestVersion.CoverUrl;

        public string Hash => beatMap.LatestVersion.Hash;

        public DateTime Uploaded => beatMap.Uploaded;

        public decimal Bpm => Math.Round(beatMap.Metadata.Bpm, 0);

        public decimal Duration => beatMap.Metadata.Duration;

        public int Upvotes => beatMap.Stats.Upvotes;

        public int Downvotes => beatMap.Stats.Downvotes;

        public string Qualified => beatMap.Qualified ? Resources.Yes : Resources.No;

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
        public List<PlaylistSongDifficultyViewModel> Difficulties { get; }

        /// <summary>
        /// Gets the command used to copy the bsr key.
        /// </summary>
        public RelayCommand CopyBsrKeyCommand { get; }

        /// <summary>
        /// Gets the command used to show the song preview in the default webbrowser.
        /// </summary>
        public RelayCommand ShowPreviewCommand { get; }

        #endregion

        /// <summary>
        /// Initialies a new <see cref="PlaylistSongDetailViewModel"/>.
        /// </summary>
        /// <param name="beatMap">The beatmap of the song.</param>
        public PlaylistSongDetailViewModel(BeatMap beatMap)
        {
            this.beatMap = beatMap;
            CopyBsrKeyCommand = new RelayCommand(CopyBsrKey);
            ShowPreviewCommand = new RelayCommand(ShowPreview);

            var playlistSongDifficulties = beatMap.LatestVersion.Difficulties.Select(d => new PlaylistSongDifficulty
            {
                Characteristic = d.Characteristic,
                Name = d.Diff,
                NPS = Math.Round(d.Nps, 1).ToString(),
                Chroma = d.Chroma,
                Noodle = d.Noodle,
                MappingExtensions = d.MappingExtension,
                Stars = d.Stars,
                Label = d.Label
            });
            Difficulties = new List<PlaylistSongDifficultyViewModel>(playlistSongDifficulties.Select(d => new PlaylistSongDifficultyViewModel(d, false)));
        }

        #region Helper methods

        private void CopyBsrKey()
        {
            try
            {
                Clipboard.SetText($"!bsr {beatMap.Id}");
            }
            catch (Exception)
            {
                var messageBoxViewModel = new MessageBoxViewModel(Resources.OK, MessageBoxButtonColor.Default, String.Empty, MessageBoxButtonColor.Default)
                {
                    Title = Resources.SongDetail_CopyBSR_Error_Title,
                    Message = Resources.SongDetail_CopyBSR_Error_Message,
                    MessageBoxType = DataAccess.Entities.Types.MessageBoxTypes.Information
                };
                MessageBoxController.Instance().ShowMessageBox(messageBoxViewModel);
            }
        }

        private void ShowPreview()
        {
            System.Diagnostics.Process.Start($"https://skystudioapps.com/bs-viewer/?id={beatMap.Id}");
        }

        #endregion
    }
}