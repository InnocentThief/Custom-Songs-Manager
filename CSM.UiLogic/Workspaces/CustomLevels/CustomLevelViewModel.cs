using CSM.DataAccess.Entities.Offline;
using System;
using System.Linq;

namespace CSM.UiLogic.Workspaces.CustomLevels
{
    public class CustomLevelViewModel
    {
        private readonly CustomLevel customLevel;

        #region Public Properties

        public string BsrKey => customLevel.BsrKey;

        public string Name => customLevel.SongName;

        public string SongSubName => customLevel.SongSubName;

        public string Mapper => customLevel.LevelAuthorName;

        public string Compser => customLevel.SongAuthorName;

        public bool Easy => customLevel.DifficultiySets.SelectMany(ds => ds.Difficulties).Any(d => d.DifficultyRank == 1);

        public bool Normal => customLevel.DifficultiySets.SelectMany(ds => ds.Difficulties).Any(d => d.DifficultyRank == 3);

        public bool Hard => customLevel.DifficultiySets.SelectMany(ds => ds.Difficulties).Any(d => d.DifficultyRank == 5);

        public bool Expert => customLevel.DifficultiySets.SelectMany(ds => ds.Difficulties).Any(d => d.DifficultyRank == 7);

        public bool ExpertPlus => customLevel.DifficultiySets.SelectMany(ds => ds.Difficulties).Any(d => d.DifficultyRank == 9);

        public DateTime ChangeDate => customLevel.ChangeDate;

        #endregion

        public CustomLevelViewModel(CustomLevel customLevel)
        {
            this.customLevel = customLevel;
        }
    }
}