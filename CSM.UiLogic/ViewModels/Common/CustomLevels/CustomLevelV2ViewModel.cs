using CSM.DataAccess.Common;
using CSM.DataAccess.CustomLevels;
using CSM.Framework.ServiceLocation;

namespace CSM.UiLogic.ViewModels.Common.CustomLevels
{
    internal class CustomLevelV2ViewModel : BaseCustomLevelViewModel<InfoV2>
    {
        public override string Version => Model.Version;

        public override string SongTitle => Model.SongName;

        public override string SongSubTitle => Model.SongSubName;

        public override string SongAuthor => Model.SongAuthorName;

        public override string LevelAuthor => Model.LevelAuthorName;

        public override double Bpm => Math.Round(Model.BeatsPerMinute, 0);

        public override bool HasEasyMap => Model.DifficultyBeatmapSets.Any(ds => ds.DifficultyBeatmaps.Any(d => d.Difficulty == Difficulty.Easy));

        public override bool HasNormalMap => Model.DifficultyBeatmapSets.Any(ds => ds.DifficultyBeatmaps.Any(d => d.Difficulty == Difficulty.Normal));

        public override bool HasHardMap => Model.DifficultyBeatmapSets.Any(ds => ds.DifficultyBeatmaps.Any(d => d.Difficulty == Difficulty.Hard));

        public override bool HasExpertMap => Model.DifficultyBeatmapSets.Any(ds => ds.DifficultyBeatmaps.Any(d => d.Difficulty == Difficulty.Expert));

        public override bool HasExpertPlusMap => Model.DifficultyBeatmapSets.Any(ds => ds.DifficultyBeatmaps.Any(d => d.Difficulty == Difficulty.ExpertPlus));

        public CustomLevelV2ViewModel(IServiceLocator serviceLocator, InfoV2 model, string path, string bsrKey, DateTime lastwriteTime) : base(serviceLocator, model, path, bsrKey, lastwriteTime)
        {
        }
    }
}
