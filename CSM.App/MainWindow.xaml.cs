using CSM.UiLogic;
using CSM.UiLogic.Workspaces;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace CSM.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RadWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            //Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("de-CH");
            DataContext = new MainWindowViewModel();

            IconTemplate = this.Resources["WindowIconTemplate"] as DataTemplate;
        }

        private void RadNavigationView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Unload removed workspace
            if (e.RemovedItems.Count > 0)
            {
                if (e.RemovedItems[0] is BaseWorkspaceViewModel workspaceViewModel) workspaceViewModel.UnloadData();
            }

            // Load added workspace
            var viewModel = DataContext as MainWindowViewModel;
            viewModel.LoadWorkspace();
        }
    }
}