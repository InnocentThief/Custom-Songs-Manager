using CSM.UiLogic;
using CSM.UiLogic.Workspaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace CSM.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            SetStyle();
            InitializeComponent();
            DataContext = new MainWindowViewModel();
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

        private async void RadNavigationView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Unload removed workspace
            if (e.RemovedItems.Count > 0)
            {
                var workspaceViewModel = e.RemovedItems[0] as BaseWorkspaceViewModel;
                workspaceViewModel.UnloadData();
            }

            // Load added workspace
            var viewModel = DataContext as MainWindowViewModel;
            await viewModel.LoadWorkspaceAsync();
        }
    }
}
