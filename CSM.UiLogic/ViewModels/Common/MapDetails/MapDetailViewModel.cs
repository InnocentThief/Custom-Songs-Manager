using CSM.DataAccess.BeatSaver;
using System.Resources;

namespace CSM.UiLogic.ViewModels.Common.MapDetails
{
    internal class MapDetailViewModel
    {
        private readonly MapDetail mapDetail;

        #region Properties

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

        public string Ranked => mapDetail.Ranked ? "Yes" : "No"; // todo: yes/no for ss and bl

        public string Qualified => mapDetail.Qualified ? "Yes" : "No";

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

        public MapDetailViewModel(MapDetail mapDetail)
        {
            this.mapDetail = mapDetail;


            var mapVersions = mapDetail.Versions.OrderByDescending(v => v.CreatedAt).ToList();
            foreach (var mapVersion in mapVersions)
            {
                Difficulties.AddRange(mapVersion.Diffs.Select(d => new MapDifficultyViewModel(d, mapVersion)));
            }
        }
    }
}
