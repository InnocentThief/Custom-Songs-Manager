﻿using CSM.UiLogic.Workspaces;
using CSM.UiLogic.Workspaces.CustomLevels;
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
            if (e.AddedItems.Count == 0) return;
            var viewModel = DataContext as CustomLevelsViewModel;
            if (viewModel?.SelectedCustomLevel != null)
            {
                await viewModel.GetBeatSaverBeatMapDataAsync(viewModel.SelectedCustomLevel.BsrKey);
            }
        }

        private void RadGridView_FilterOperatorsLoading(object sender, Telerik.Windows.Controls.GridView.FilterOperatorsLoadingEventArgs e)
        {
            if (e.Column.UniqueName == "BsrKey")
            {
                e.AvailableOperators.Remove(Telerik.Windows.Data.FilterOperator.IsLessThan);
                e.AvailableOperators.Remove(Telerik.Windows.Data.FilterOperator.IsLessThanOrEqualTo);
                e.AvailableOperators.Remove(Telerik.Windows.Data.FilterOperator.IsNotEqualTo);
                e.AvailableOperators.Remove(Telerik.Windows.Data.FilterOperator.IsGreaterThanOrEqualTo);
                e.AvailableOperators.Remove(Telerik.Windows.Data.FilterOperator.IsGreaterThan);
                e.AvailableOperators.Remove(Telerik.Windows.Data.FilterOperator.StartsWith);
                e.AvailableOperators.Remove(Telerik.Windows.Data.FilterOperator.EndsWith);
                e.AvailableOperators.Remove(Telerik.Windows.Data.FilterOperator.Contains);
                e.AvailableOperators.Remove(Telerik.Windows.Data.FilterOperator.DoesNotContain);
                e.AvailableOperators.Remove(Telerik.Windows.Data.FilterOperator.IsContainedIn);
                e.AvailableOperators.Remove(Telerik.Windows.Data.FilterOperator.IsNotContainedIn);
                e.DefaultOperator1 = Telerik.Windows.Data.FilterOperator.IsEqualTo;
            }
            else
            {
                e.DefaultOperator1 = Telerik.Windows.Data.FilterOperator.Contains;
            }
        }
    }
}