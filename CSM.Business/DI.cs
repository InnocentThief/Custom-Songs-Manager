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
                    .AddSingleton<IBeatLeaderService, BeatLeaderService>()
                    .AddSingleton<IBeatSaverService, BeatSaverService>()
                    .AddSingleton<IScoreSaberService, ScoreSaberService>()
                    .AddSingleton<ISongCopyDomain, SongCopyDomain>()
                    .AddSingleton<ISongSelectionDomain, SongSelectionDomain>()
                    .AddSingleton<ISongSuggestDomain, SongSuggestDomain>()
                    .AddSingleton<ITwitchService, TwitchService>()
                    .AddSingleton<ITwitchChannelService, TwitchChannelService>()
                    .AddSingleton<IUserConfigDomain, UserConfigDomain>();
        }
    }
}
