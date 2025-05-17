using CSM.App.Views.Helper;
using CSM.App.Views.Windows;
using CSM.Business;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telerik.Windows.Controls;
using Telerik.Windows.Persistence.Services;
using Telerik.Windows.Persistence;

namespace CSM.App
{
    public static class DI
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration, IServiceLocator serviceLocator)
        {
            return services
                .AddBusiness()
                .AddUiLogic()
                .AddSingleton(serviceLocator)
                .AddTransient<EditWindow>();
        }

        #region Helper methods


        #endregion
    }
}
