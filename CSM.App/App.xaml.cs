using CSM.App.Wizards;
using CSM.Framework.Logging;
using CSM.UiLogic;
using CSM.UiLogic.Wizards;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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

            var editWindowController = EditWindowController.Instance();
            editWindowController.ShowEditWindowEvent += EditWindowController_ShowEditWindowEvent;

            var messageBoxController = MessageBoxController.Instance();
            messageBoxController.ShowMessageBoxEvent += MessageBoxController_ShowMessageBoxEvent;
        }

        private void EditWindowController_ShowEditWindowEvent(object sender, EditWindowEventArgs e)
        {
            var editWindow = new EditWizardWindow();
            var viewModel = e.EditWindowViewModel;
            viewModel.CloseAction = new Action(() => editWindow.Close());
            editWindow.DataContext = e.EditWindowViewModel;
            editWindow.Owner = Current.MainWindow;
            editWindow.ShowDialog();
        }

        private void MessageBoxController_ShowMessageBoxEvent(object sender, MessageBoxEventArgs e)
        {
            var messageBox = new MessageBoxWindow();
            var viewModel = e.MessageBoxViewModel;
            viewModel.CloseAction = new Action(() => messageBox.Close());
            messageBox.DataContext = e.MessageBoxViewModel;
            messageBox.Owner = Current.MainWindow;
            messageBox.ShowDialog();
        }

        protected sealed override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SetStyle();

            MainWindow window = new MainWindow();
            var viewModel = new MainWindowViewModel();
            window.DataContext = viewModel;
            window.Show();
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

            //StyleManager.ApplicationTheme = new Windows11Theme();
            //Windows11Palette.Palette.PrimaryForegroundColor = (Color)ColorConverter.ConvertFromString("#FFFFFFFF");
            //Windows11Palette.Palette.SecondaryForegroundColor = (Color)ColorConverter.ConvertFromString("#C8FFFFFF");
            //Windows11Palette.Palette.TertiaryForegroundColor = (Color)ColorConverter.ConvertFromString("#8BFFFFFF");
            //Windows11Palette.Palette.DisabledForegroundColor = (Color)ColorConverter.ConvertFromString("#FFFFFFFF");
            //Windows11Palette.Palette.AccentControlForegroundColor = (Color)ColorConverter.ConvertFromString("#FFFFFFFF");
            //Windows11Palette.Palette.IconColor = (Color)ColorConverter.ConvertFromString("#FFFFFFFF");
            //Windows11Palette.Palette.IconSecondaryColor = (Color)ColorConverter.ConvertFromString("#C8FFFFFF");
            //Windows11Palette.Palette.PrimaryBackgroundColor = (Color)ColorConverter.ConvertFromString("#0FFFFFFF");
            //Windows11Palette.Palette.PrimarySolidBackgroundColor = (Color)ColorConverter.ConvertFromString("#FF1E1E1E");
            //Windows11Palette.Palette.SecondaryBackgroundColor = (Color)ColorConverter.ConvertFromString("#FF1C1C1C");
            //Windows11Palette.Palette.TertiaryBackgroundColor = (Color)ColorConverter.ConvertFromString("#FF282828");
            //Windows11Palette.Palette.TertiarySmokeBackgroundColor = (Color)ColorConverter.ConvertFromString("#4D000000");
            //Windows11Palette.Palette.SubtleColor = (Color)ColorConverter.ConvertFromString("#0FFFFFFF");
            //Windows11Palette.Palette.SubtleSecondaryColor = (Color)ColorConverter.ConvertFromString("#0BFFFFFF");
            //Windows11Palette.Palette.AlternativeColor = (Color)ColorConverter.ConvertFromString("#FF202020");
            //Windows11Palette.Palette.OverlayColor = (Color)ColorConverter.ConvertFromString("#FF2D2D2D");
            //Windows11Palette.Palette.PrimaryBorderColor = (Color)ColorConverter.ConvertFromString("#12FFFFFF");
            //Windows11Palette.Palette.PrimarySolidBorderColor = (Color)ColorConverter.ConvertFromString("#FF2C2C2C");
            //Windows11Palette.Palette.SecondaryBorderColor = (Color)ColorConverter.ConvertFromString("#18FFFFFF");
            //Windows11Palette.Palette.TertiaryBorderColor = (Color)ColorConverter.ConvertFromString("#FF262626");
            //Windows11Palette.Palette.ButtonBorderGradientStop1Color = (Color)ColorConverter.ConvertFromString("#17FFFFFF");
            //Windows11Palette.Palette.ButtonBorderGradientStop2Color = (Color)ColorConverter.ConvertFromString("#11FFFFFF");
            //Windows11Palette.Palette.InputBorderGradientStop1Color = (Color)ColorConverter.ConvertFromString("#14FFFFFF");
            //Windows11Palette.Palette.InputBorderGradientStop2Color = (Color)ColorConverter.ConvertFromString("#8AFFFFFF");
            //Windows11Palette.Palette.AccentControlBorderGradientStop1Color = (Color)ColorConverter.ConvertFromString("#14FFFFFF");
            //Windows11Palette.Palette.AccentControlBorderGradientStop2Color = (Color)ColorConverter.ConvertFromString("#23000000");
            //Windows11Palette.Palette.FocusColor = (Color)ColorConverter.ConvertFromString("#FFFFFFFF");
            //Windows11Palette.Palette.FocusInnerColor = (Color)ColorConverter.ConvertFromString("#B3000000");
            //Windows11Palette.Palette.MouseOverBackgroundColor = (Color)ColorConverter.ConvertFromString("#15FFFFFF");
            //Windows11Palette.Palette.MouseOverBorderGradientStop1Color = (Color)ColorConverter.ConvertFromString("#14FFFFFF");
            //Windows11Palette.Palette.MouseOverBorderGradientStop2Color = (Color)ColorConverter.ConvertFromString("#8AFFFFFF");
            //Windows11Palette.Palette.PressedBackgroundColor = (Color)ColorConverter.ConvertFromString("#08FFFFFF");
            //Windows11Palette.Palette.SelectedColor = (Color)ColorConverter.ConvertFromString("#0FFFFFFF");
            //Windows11Palette.Palette.SelectedMouseOverColor = (Color)ColorConverter.ConvertFromString("#0BFFFFFF");
            //Windows11Palette.Palette.SelectedUnfocusedColor = (Color)ColorConverter.ConvertFromString("#FF404040");
            //Windows11Palette.Palette.StrokeColor = (Color)ColorConverter.ConvertFromString("#FF313131");
            //Windows11Palette.Palette.ReadOnlyBackgroundColor = (Color)ColorConverter.ConvertFromString("#08FFFFFF");
            //Windows11Palette.Palette.ReadOnlyBorderColor = (Color)ColorConverter.ConvertFromString("#FF1C1C1C");
            //Windows11Palette.Palette.DisabledBackgroundColor = (Color)ColorConverter.ConvertFromString("#0BFFFFFF");
            //Windows11Palette.Palette.DisabledBorderColor = (Color)ColorConverter.ConvertFromString("#12FFFFFF");
            //Windows11Palette.Palette.ValidationColor = (Color)ColorConverter.ConvertFromString("#FFFF99A4");
            //Windows11Palette.Palette.AccentColor = (Color)ColorConverter.ConvertFromString("#FF60CDFF");
            //Windows11Palette.Palette.AccentMouseOverColor = (Color)ColorConverter.ConvertFromString("#E660CDFF");
            //Windows11Palette.Palette.AccentPressedColor = (Color)ColorConverter.ConvertFromString("#CC60CDFF");
            //Windows11Palette.Palette.AccentSelectedColor = (Color)ColorConverter.ConvertFromString("#FF0078D4");
            //Windows11Palette.Palette.DisabledOpacity = 0.36;
            //Windows11Palette.Palette.ReadOnlyOpacity = 0.79;



        }


    }
}
