using CSM.UiLogic;
using CSM.UiLogic.Workspaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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
            InitializeComponent();
            //Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("de-CH");
            DataContext = new MainWindowViewModel();
        }

        private void RadNavigationView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Unload removed workspace
            if (e.RemovedItems.Count > 0)
            {
                var workspaceViewModel = e.RemovedItems[0] as BaseWorkspaceViewModel;
                workspaceViewModel.UnloadData();
            }

            // Load added workspace
            var viewModel = DataContext as MainWindowViewModel;
            viewModel.LoadWorkspace();
        }
    }
}
