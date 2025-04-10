using CSM.UiLogic.Commands;
using CSM.UiLogic.ViewModels;
using CSM.UiLogic.ViewModels.Workspaces;
using Microsoft.Extensions.DependencyInjection;

namespace CSM.UiLogic
{
    internal static class DI
    {
        public static IServiceCollection AddUiLogic(this IServiceCollection services)
        {
            return services
                .AddViewModels();
        }

        #region Helper methods

        private static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            return services
                .AddSingleton<ICommandFactory, CommandFactory>()
                .AddWorkspaces()
                .AddSingleton<MainWindowViewModel>();
        }

        private static IServiceCollection AddWorkspaces(this IServiceCollection services)
        {
            return services
                .AddTransient<CustomLevelsWorkspaceViewModel>()
                .AddTransient<PlaylistsWorkspaceViewModel>()
                .AddTransient<TwitchWorkspaceViewModel>()
                .AddTransient<ScoreSaberWorkspaceViewModel>()
                .AddTransient<BeatLeaderWorkspaceViewModel>();
        }

        #endregion
    }
}
