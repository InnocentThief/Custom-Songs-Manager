using CSM.Business;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CSM.App
{
    public static class DI
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration, IServiceLocator serviceLocator)
        {
            return services
                .AddBusiness()
                .AddUiLogic()
                .AddSingleton(serviceLocator);
        }

        #region Helper methods


        #endregion
    }
}
