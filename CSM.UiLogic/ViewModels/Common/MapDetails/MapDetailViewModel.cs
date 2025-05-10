using System.Diagnostics;
using System.Text;
using System.Windows;
using CSM.DataAccess.BeatSaver;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;

namespace CSM.UiLogic.ViewModels.Common.MapDetails
{
    internal class MapDetailViewModel : BaseViewModel
    {
        #region Private fields

        private IRelayCommand? previewCommand, copyBSRCommand;
        private readonly MapDetail mapDetail;

        #endregion

        #region Properties

        public IRelayCommand? PreviewCommand => previewCommand ??= CommandFactory.Create(Preview, CanPreview);
        public IRelayCommand? CopyBSRCommand => copyBSRCommand ??= CommandFactory.Create(CopyBSR, CanCopyBSR);

        public MapDetail Model => mapDetail;

        public string Id => mapDetail.Id;

        public string CoverUrl => mapDetail.Versions.OrderByDescending(v => v.CreatedAt).FirstOrDefault()?.CoverUrl ?? string.Empty;

        public string SongName => mapDetail.Metadata?.SongName ?? string.Empty;

        public string SongSubName => mapDetail.Metadata?.SongSubName ?? string.Empty;

        public string LevelAuthorName => mapDetail.Metadata?.LevelAuthorName ?? string.Empty;

        public string SongAuthorName => mapDetail.Metadata?.SongAuthorName ?? string.Empty;

        public DateTime Uploaded => mapDetail.Uploaded;

        public decimal? Duration => mapDetail.Metadata?.Duration;

        public decimal? Bpm => Math.Round(mapDetail.Metadata?.Bpm ?? 0, 0);

        public int? Upvotes => mapDetail.Stats?.Upvotes;

        public int? Downvotes => mapDetail.Stats?.Downvotes;

        public string Score
        {
            get
            {
                if (mapDetail.Stats == null) return string.Empty;
                return $"{Math.Round(mapDetail.Stats.Score * 100, 0)}%";
            }
        }

        public string Ranked
        {
            get
            {
                var ranked = new StringBuilder();
                if (mapDetail.Versions.Any(v => v.Diffs.Any(d => d.Stars > 0)))
                    ranked.Append("SS");

                if (mapDetail.Versions.Any(v => v.Diffs.Any(d => d.BlStars > 0)))
                {
                    if (ranked.Length == 0)
                    {
                        ranked.Append("BL");
                    }
                    else
                    {
                        ranked.Append(", BL");
                    }
                }

                if (string.IsNullOrWhiteSpace(ranked.ToString()))
                {
                    return "No";
                }
                else
                {
                    return ranked.ToString();
                }
            }
        }

        public string Qualified
        {
            get
            {
                if (mapDetail.Ranked)
                {
                    var ranked = new StringBuilder();
                    if (mapDetail.Versions.Any(v => v.Diffs.Any(d => d.Stars > 0)))
                        ranked.Append("SS");

                    if (mapDetail.Versions.Any(v => v.Diffs.Any(d => d.BlStars > 0)))
                    {
                        if (ranked.Length == 0)
                        {
                            ranked.Append("BL");
                        }
                        else
                        {
                            ranked.Append(", BL");
                        }
                    }
                    return ranked.ToString();
                }
                return "No";
            }
        }

        public string Tags
        {
            get
            {
                if (mapDetail == null) return string.Empty;
                if (mapDetail.Tags == null) return string.Empty;
                return String.Join(", ", mapDetail.Tags);
            }
        }

        public string Description => mapDetail.Description;

        public bool HasDescription => !string.IsNullOrWhiteSpace(mapDetail.Description);

        public List<MapDifficultyViewModel> Difficulties { get; } = [];

        #endregion

        public MapDetailViewModel(IServiceLocator serviceLocator, MapDetail mapDetail) : base(serviceLocator)
        {
            this.mapDetail = mapDetail;

            var mapVersions = mapDetail.Versions.OrderByDescending(v => v.CreatedAt).ToList();
            foreach (var mapVersion in mapVersions)
            {
                Difficulties.AddRange(mapVersion.Diffs.Select(d => new MapDifficultyViewModel(serviceLocator, d, mapVersion)));
            }
        }

        #region Helper methods

        private void Preview()
        {
            string url = $"https://allpoland.github.io/ArcViewer/?id={mapDetail.Id}";
            if (!string.IsNullOrWhiteSpace(url))
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
        }

        private bool CanPreview()
        {
            return true;
        }

        private void CopyBSR()
        {
            Clipboard.SetText($"!BSR {mapDetail.Id}");
        }

        private bool CanCopyBSR()
        {
            return !string.IsNullOrWhiteSpace(mapDetail.Id);
        }

        #endregion
    }
}
