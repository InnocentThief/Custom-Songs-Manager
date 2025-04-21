using CSM.DataAccess.BeatSaver;

namespace CSM.UiLogic.ViewModels.Common.MapDetails
{
    internal class MapDetailViewModel
    {
        private readonly MapDetail mapDetail;

        #region Properties

        public string CoverUrl => mapDetail.Versions.OrderByDescending(v => v.CreatedAt).FirstOrDefault()?.CoverUrl ?? string.Empty;

        public string SongName => mapDetail.Metadata?.SongName ?? string.Empty;

        public string SongSubName => mapDetail.Metadata?.SongSubName ?? string.Empty;

        public string LevelAuthorName => mapDetail.Metadata?.LevelAuthorName ?? string.Empty;

        public string SongAuthorName => mapDetail.Metadata?.SongAuthorName ?? string.Empty;

        #endregion

        public MapDetailViewModel(MapDetail mapDetail)
        {
            this.mapDetail = mapDetail;
        }
    }
}
