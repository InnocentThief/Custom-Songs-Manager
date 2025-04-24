using CSM.Business.Interfaces;
using CSM.DataAccess.BeatSaver;
using CSM.DataAccess.Common;
using CSM.DataAccess.UserConfiguration;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Common.MapDetails
{
    internal class MapDifficultyViewModel(IServiceLocator serviceLocator, MapDifficulty difficulty, MapVersion version) : BaseViewModel(serviceLocator)
    {
        #region Private fields

        private readonly MapDifficulty difficulty = difficulty;
        private readonly MapVersion version = version;
        private readonly IUserConfigDomain userConfigDomain = serviceLocator.GetService<IUserConfigDomain>();

        #endregion

        public Characteristic Characteristic => difficulty.Characteristic;

        public Difficulty Difficulty => difficulty.Difficulty;

        public string Label => difficulty.Label;

        public string Infos
        {
            get
            {
                var infos = new List<string>();
                infos.Add($"NPS: {Math.Round(difficulty.Nps, 2)}");
                infos.Add($"NJS: {Math.Round(difficulty.Njs, 2)}");
                if (difficulty.Chroma)
                    infos.Add("CR");
                if (difficulty.Me)
                    infos.Add("ME");
                if (difficulty.Ne)
                    infos.Add("NE");
                return string.Join(", ", infos);
            }
        }

        public string Stars
        {
            get
            {
                switch (userConfigDomain.Config?.VisibleLeaderboardInfos)
                {
                    case LeaderboardType.None:
                        return string.Empty;
                    case LeaderboardType.ScoreSaber:
                        if (difficulty.Stars == 0)
                            return string.Empty;
                        return $"{Math.Round(difficulty.Stars, 2)}*";
                    case LeaderboardType.BeatLeader:
                        if (difficulty.BlStars == 0)
                            return string.Empty;
                        return $"{Math.Round(difficulty.BlStars, 2)}*";
                    default:
                        return string.Empty;
                }
            }
        }
    }
}
