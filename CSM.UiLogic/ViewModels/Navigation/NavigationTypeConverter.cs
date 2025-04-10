using CSM.Framework;
using CSM.Framework.Types;
using CSM.UiLogic.ViewModels.Workspaces;

namespace CSM.UiLogic.ViewModels.Navigation
{
    internal static class NavigationTypeConverter
    {
        public static Type? ToViewModelType(this NavigationType navigationType)
        {
            return navigationType switch
            {
                NavigationType.CustomLevels => typeof(CustomLevelsWorkspaceViewModel),
                NavigationType.Playlists => typeof(PlaylistsWorkspaceViewModel),
                NavigationType.TwitchIntegration => typeof(TwitchWorkspaceViewModel),
                NavigationType.ScoreSaberIntegration => typeof(ScoreSaberWorkspaceViewModel),
                NavigationType.BeatLeaderIntegration => typeof(BeatLeaderWorkspaceViewModel),
                _ => throw new NotImplementedException($"View model type for navigation type '{navigationType}' missing"),
            };
        }
    }
}
