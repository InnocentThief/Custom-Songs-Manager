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

namespace CSM.App.Workspaces.CustomLevels
{
    /// <summary>
    /// Interaction logic for CustomLevelList.xaml
    /// </summary>
    public partial class CustomLevelListView : UserControl
    {
        public CustomLevelListView()
        {
            InitializeComponent();
        }

        private async void RadGridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            var viewModel = DataContext as CustomLevelsViewModel;
            if (viewModel?.SelectedCustomLevel != null)
            {
                await viewModel.GetBeatSaverBeatMapDataAsync(viewModel.SelectedCustomLevel.BsrKey);
            }
        }
    }
}