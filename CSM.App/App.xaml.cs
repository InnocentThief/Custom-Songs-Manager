using CSM.Framework.Logging;
using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Controls;

namespace CSM.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Private fields


        #endregion

        /// <summary>
        /// Initializes a new <see cref="App"/>.
        /// </summary>
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Logger.Create();
        }

        protected sealed override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SetStyle();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LoggerProvider.Logger.Fatal<App>($"Exception was unhandled in current app domain: {e.ExceptionObject}");
        }

        private void SetStyle()
        {
            StyleManager.ApplicationTheme = new FluentTheme();
            FluentPalette.Palette.AccentColor = (Color)ColorConverter.ConvertFromString("#FF018574");
            FluentPalette.Palette.AccentFocusedColor = (Color)ColorConverter.ConvertFromString("#FF0AE8CB");
            FluentPalette.Palette.AccentMouseOverColor = (Color)ColorConverter.ConvertFromString("#FF00B294");
            FluentPalette.Palette.AccentPressedColor = (Color)ColorConverter.ConvertFromString("#FF017566");
            FluentPalette.Palette.AlternativeColor = (Color)ColorConverter.ConvertFromString("#FF2B2B2B");
            FluentPalette.Palette.BasicColor = (Color)ColorConverter.ConvertFromString("#4CFFFFFF");
            FluentPalette.Palette.BasicSolidColor = (Color)ColorConverter.ConvertFromString("#FF4D4D4D");
            FluentPalette.Palette.ComplementaryColor = (Color)ColorConverter.ConvertFromString("#FF333333");
            FluentPalette.Palette.IconColor = (Color)ColorConverter.ConvertFromString("#CCFFFFFF");
            FluentPalette.Palette.MainColor = (Color)ColorConverter.ConvertFromString("#33FFFFFF");
            FluentPalette.Palette.MarkerColor = (Color)ColorConverter.ConvertFromString("#FFFFFFFF");
            FluentPalette.Palette.MarkerInvertedColor = (Color)ColorConverter.ConvertFromString("#FFFFFFFF");
            FluentPalette.Palette.MarkerMouseOverColor = (Color)ColorConverter.ConvertFromString("#FF000000");
            FluentPalette.Palette.MouseOverColor = (Color)ColorConverter.ConvertFromString("#4CFFFFFF");
            FluentPalette.Palette.PressedColor = (Color)ColorConverter.ConvertFromString("#26FFFFFF");
            FluentPalette.Palette.PrimaryBackgroundColor = (Color)ColorConverter.ConvertFromString("#FF000000");
            FluentPalette.Palette.PrimaryColor = (Color)ColorConverter.ConvertFromString("#66FFFFFF");
            FluentPalette.Palette.PrimaryMouseOverColor = (Color)ColorConverter.ConvertFromString("#FFFFFFFF");
            FluentPalette.Palette.ReadOnlyBackgroundColor = (Color)ColorConverter.ConvertFromString("#00FFFFFF");
            FluentPalette.Palette.ReadOnlyBorderColor = (Color)ColorConverter.ConvertFromString("#FF4C4C4C");
            FluentPalette.Palette.ValidationColor = (Color)ColorConverter.ConvertFromString("#FFE81123");
            FluentPalette.Palette.DisabledOpacity = 0.3;
            FluentPalette.Palette.InputOpacity = 0.6;
            FluentPalette.Palette.ReadOnlyOpacity = 0.5;

        }


    }
}
