using CSM.DataAccess.BeatSaver;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using System.Text;

namespace CSM.UiLogic.ViewModels.Common.MapDetails
{
    internal class MapDetailViewModel : BaseViewModel
    {
        private readonly MapDetail mapDetail;

        #region Properties

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

        //public string Score => $"{Math.Round(mapDetail.Stats?.Score ?? 0 * 100, 0)}%";

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
    }
}
