using CSM.DataAccess.Common;
using CSM.DataAccess.CustomLevels;
using CSM.Framework.ServiceLocation;

namespace CSM.UiLogic.ViewModels.Common.CustomLevels
{
    internal sealed class CustomLevelV4ViewModel(
        IServiceLocator serviceLocator, 
        InfoV4 model, 
        string path, 
        string bsrKey, 
        DateTime lastwriteTime) 
        : BaseCustomLevelViewModel<InfoV4>(serviceLocator, model, path, bsrKey, lastwriteTime)
    {
        public override string Version => Model.Version;

        public override string SongTitle => Model.Song.Title;

        public override string SongSubTitle => Model.Song.SubTitle;

        public override string SongAuthor => Model.Song.Author;

        public override string LevelAuthor => string.Join(", ", Model.DifficultyBeatmaps.Select(dbm => dbm.BeatmapAuthors?.Mappers));

        public override double Bpm => Math.Round(Model.Audio.Bpm, 0);

        public override bool HasEasyMap => Model.DifficultyBeatmaps?.Any(dbm => dbm.Difficulty == Difficulty.Easy) ?? false;

        public override bool HasNormalMap => Model.DifficultyBeatmaps?.Any(dbm => dbm.Difficulty == Difficulty.Normal) ?? false;

        public override bool HasHardMap => Model.DifficultyBeatmaps?.Any(dbm => dbm.Difficulty == Difficulty.Hard) ?? false;

        public override bool HasExpertMap => Model.DifficultyBeatmaps?.Any(dbm => dbm.Difficulty == Difficulty.Expert) ?? false;

        public override bool HasExpertPlusMap => Model.DifficultyBeatmaps?.Any(dbm => dbm.Difficulty == Difficulty.ExpertPlus) ?? false;
    }
}
