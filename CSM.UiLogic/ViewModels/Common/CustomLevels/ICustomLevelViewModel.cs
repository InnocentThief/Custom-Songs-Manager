using CSM.DataAccess.BeatSaver;

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

        void UpdateMapDetail(MapDetail mapDetail);
    }
}
