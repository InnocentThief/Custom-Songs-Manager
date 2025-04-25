using CSM.DataAccess.BeatSaver;
using CSM.UiLogic.ViewModels.Common.MapDetails;

namespace CSM.UiLogic.ViewModels.Common.CustomLevels
{
    interface ICustomLevelViewModel
    {
        string Path { get; }
        string BsrKey { get; }
        DateTime LastWriteTime { get; }
        string Version { get; }
        string SongTitle { get; }
        string SongSubTitle { get; }
        string SongAuthor { get; }
        MapDetailViewModel? MapDetailViewModel { get; }

        void CleanUpReferences();
        void UpdateMapDetail(MapDetail mapDetail);
    }
}
