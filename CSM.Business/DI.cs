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
                .AddDomains();
        }

        #region Helper methods

        private static IServiceCollection AddDomains(this IServiceCollection services)
        {
            return services
                    .AddSingleton<IUserConfigDomain, UserConfigDomain>()
                    .AddSingleton<ISongSuggestDomain, SongSuggestDomain>();
        }

        #endregion
    }
}
