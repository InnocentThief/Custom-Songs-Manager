using CSM.UiLogic.Workspaces;
using System.Windows.Controls;

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

        private void RadGridView_FilterOperatorsLoading(object sender, Telerik.Windows.Controls.GridView.FilterOperatorsLoadingEventArgs e)
        {
            e.DefaultOperator1 = Telerik.Windows.Data.FilterOperator.Contains;
        }
    }
}