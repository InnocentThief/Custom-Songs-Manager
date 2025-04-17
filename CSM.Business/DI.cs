using CSM.Business.Core;
using CSM.Business.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CSM.Business
{
    internal static class DI
    {
        public static IServiceCollection AddBusiness(this IServiceCollection services)
        {
            return services
                    .AddSingleton<ISongCopyDomain, SongCopyDomain>()
                    .AddSingleton<ISongSuggestDomain, SongSuggestDomain>()
                    .AddSingleton<IUserConfigDomain, UserConfigDomain>()
                    .AddSingleton<IBeatLeaderService, BeatLeaderService>()
                    .AddSingleton<IBeatSaverService, BeatSaverService>()
                    .AddSingleton<IScoreSaberService, ScoreSaberService>();
        }
    }
}
