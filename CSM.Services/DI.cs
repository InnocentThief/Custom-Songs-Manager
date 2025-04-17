using CSM.Services.Core;
using CSM.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CSM.Services
{
    internal static class DI
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<IBeatLeaderService, BeatLeaderService>()
                .AddSingleton<IBeatSaverService, BeatSaverService>()
                .AddSingleton<IScoreSaberService, ScoreSaberService>();
        }
    }
}
