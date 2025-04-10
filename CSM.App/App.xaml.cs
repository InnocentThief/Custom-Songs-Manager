using CSM.App.Views;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Telerik.Windows.Controls;

namespace CSM.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Private fields

        private IHost? host;
        private MainWindowViewModel? mainWindowViewModel;

        #endregion

        internal IServiceLocator? ServiceLocator { get; private set; }

        public App()
        {
            SetStyle();

            Current.DispatcherUnhandledException += CurrentDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerUnobservedTaskException;
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            host = SetupHost(e.Args);
            await host.StartAsync();

            base.OnStartup(e);

            await Task.Run(Startup);
            async void Startup()
            {
                var success = StartupApplication(host.Services);
                if (success)
                {
                    await host.RunAsync();
                    Dispatcher.Invoke(Shutdown);
                }
                else
                {
                    Dispatcher.Invoke(Shutdown);
                }
            }
        }

        #region Helper methods

        private IHost SetupHost(string[] args)
        {
            ServiceLocator = new ServiceLocator();
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((ctx, services) =>
                {
                    services
                    .ConfigureServices(ctx.Configuration, ServiceLocator);
                })
                .Build();
            ServiceLocator.SetDefaultServiceFactory(host.Services.GetService);
            return host;
        }

        private bool StartupApplication(IServiceProvider services)
        {
            if (ServiceLocator == null)
            {
                return false;
            }

            try
            {
                Dispatcher.Invoke(Startup);

                void Startup()
                {
                    mainWindowViewModel = CreateMainWindowViewModel(services);
                    if (mainWindowViewModel.CanRun)
                    {
                        var MainWindow = new MainWindow()
                        {
                            DataContext = mainWindowViewModel
                        };
                        MainWindow.Show();
                        MainWindow.Activate();
                        mainWindowViewModel.NavigateToDefaultWorkspace();
                    }
                    else
                    {
                        Shutdown();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static MainWindowViewModel CreateMainWindowViewModel(IServiceProvider sp)
        {
            return sp.GetService<MainWindowViewModel>() ?? throw new Exception("Could not resolve MainViewModel!");
        }

        private void CurrentDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if (ServiceLocator == null)
            {
                return;
            }
            //LogCurrentDispatcherUnhandledException(ServiceLocator.GetService<ILogger<App>>(), e.Exception);
            //userInteraction?.ShowError(e.Exception.ToString());
            e.Handled = true;
        }

        private void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (ServiceLocator == null)
            {
                return;
            }
            if (e.ExceptionObject is Exception ex)
            {
                //LogCurrentDomainUnhandledException(ServiceLocator.GetService<ILogger<App>>(), ex);
            }
            else
            {
                //LogCurrentDomainUnhandledExceptionFromObject(ServiceLocator.GetService<ILogger<App>>(), e.ExceptionObject);
            }
            //userInteraction?.ShowError(e.ExceptionObject.ToString() ?? string.Empty);
        }

        private void TaskSchedulerUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            if (ServiceLocator == null)
            {
                return;
            }
            //LogTaskSchedulerUnobservedTaskException(ServiceLocator.GetService<ILogger<App>>(), e.Exception);
            //userInteraction?.ShowError(e.Exception.ToString());
        }

        private void SetStyle()
        {
            StyleManager.ApplicationTheme = new Windows11Theme();
            Windows11Palette.LoadPreset(Windows11Palette.ColorVariation.Dark);
            Windows11ThemeSizeHelper.Helper.IsInCompactMode = true;

            Windows11Palette.Palette.AccentColor = (Color)ColorConverter.ConvertFromString("#FF018574");
            //Windows11Palette.Palette.AccentFocusedColor = (Color)ColorConverter.ConvertFromString("#FF0AE8CB");
            Windows11Palette.Palette.AccentMouseOverColor = (Color)ColorConverter.ConvertFromString("#FF00B294");
            Windows11Palette.Palette.AccentPressedColor = (Color)ColorConverter.ConvertFromString("#FF017566");
            Windows11Palette.Palette.AlternativeColor = (Color)ColorConverter.ConvertFromString("#FF2B2B2B");
            //Windows11Palette.Palette.BasicColor = (Color)ColorConverter.ConvertFromString("#4CFFFFFF");
            //Windows11Palette.Palette.BasicSolidColor = (Color)ColorConverter.ConvertFromString("#FF4D4D4Dk");
            //Windows11Palette.Palette.ComplementaryColor = (Color)ColorConverter.ConvertFromString("#FF333333");
            Windows11Palette.Palette.IconColor = (Color)ColorConverter.ConvertFromString("#CCFFFFFF");
            //Windows11Palette.Palette.MainColor = (Color)ColorConverter.ConvertFromString("#33FFFFFF");
            //Windows11Palette.Palette.MarkerColor = (Color)ColorConverter.ConvertFromString("#FFFFFFFF");
            //Windows11Palette.Palette.MarkerInvertedColor = (Color)ColorConverter.ConvertFromString("#FFFFFFFF");
            //Windows11Palette.Palette.MarkerMouseOverColor = (Color)ColorConverter.ConvertFromString("#FF000000");
            //Windows11Palette.Palette.MouseOverColor = (Color)ColorConverter.ConvertFromString("#4CFFFFFF");
            //Windows11Palette.Palette.PressedColor = (Color)ColorConverter.ConvertFromString("#26FFFFFF");
            Windows11Palette.Palette.PrimaryBackgroundColor = (Color)ColorConverter.ConvertFromString("#FF000000");
            //Windows11Palette.Palette.PrimaryColor = (Color)ColorConverter.ConvertFromString("#66FFFFFF");
            //Windows11Palette.Palette.PrimaryMouseOverColor = (Color)ColorConverter.ConvertFromString("#FFFFFFFF");
            Windows11Palette.Palette.ReadOnlyBackgroundColor = (Color)ColorConverter.ConvertFromString("#00FFFFFF");
            Windows11Palette.Palette.ReadOnlyBorderColor = (Color)ColorConverter.ConvertFromString("#FF4C4C4C");
            Windows11Palette.Palette.ValidationColor = (Color)ColorConverter.ConvertFromString("#FFE81123");
            Windows11Palette.Palette.DisabledOpacity = 0.3;
            //Windows11Palette.Palette.InputOpacity = 0.6;
            Windows11Palette.Palette.ReadOnlyOpacity = 0.5;


            Windows11Palette.Palette.SelectedColor = (Color)ColorConverter.ConvertFromString("#FF018574");
            Windows11Palette.Palette.AccentSelectedColor = (Color)ColorConverter.ConvertFromString("#FF018574");
        }

        #endregion
    }

}
