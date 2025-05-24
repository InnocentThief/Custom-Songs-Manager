using CSM.App.Views.Helper;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.ViewModels.Controls.CustomLevels;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls.Filtering.Editors;
using Telerik.Windows.Persistence;

namespace CSM.App.Views.Controls.CustomLevels
{
    /// <summary>
    /// Interaction logic for CustomLevelsListView.xaml
    /// </summary>
    public partial class CustomLevelsControl : UserControl
    {
        private PersistenceManager persistenceManager = PersistenceFrameworkHelper.GetPersistenceManager();

        public CustomLevelsControl()
        {
            InitializeComponent();
        }

        private async void MainCustomLevelsGridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            if (DataContext is CustomLevelsControlViewModel viewModel)
            {
                await viewModel.LoadSelectedCustomLevelDataAsync();
            }
        }

        private void MainCustomLevelsGridView_FilterOperatorsLoading(object sender, Telerik.Windows.Controls.GridView.FilterOperatorsLoadingEventArgs e)
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

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CustomLevelsControlViewModel viewModel)
            {
                var stream = persistenceManager.Save(MainCustomLevelsGridView);
                await viewModel.SaveViewDefinitionAsync(stream, SavableUiElement.CustomLevelsMainView);
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CustomLevelsControlViewModel viewModel && viewModel.SelectedViewDefinition != null)
            {
                var stream = persistenceManager.Save(MainCustomLevelsGridView);
                await viewModel.SaveViewDefinitionAsync(stream, SavableUiElement.CustomLevelsMainView, viewModel.SelectedViewDefinition.Name);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CustomLevelsControlViewModel viewModel && viewModel.SelectedViewDefinition != null)
            {
                viewModel.DeleteViewDefinition(SavableUiElement.CustomLevelsMainView, viewModel.SelectedViewDefinition.Name);
            }
        }

        private void ViewDefinitions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is CustomLevelsControlViewModel viewModel)
            {
                if (viewModel.SelectedViewDefinition != null && viewModel.SelectedViewDefinition.Stream != null)
                {
                    persistenceManager.Load(MainCustomLevelsGridView, viewModel.SelectedViewDefinition.Stream);
                    viewModel.SelectedViewDefinition.Stream.Position = 0;
                }
            }
        }

        private void MainCustomLevelsGridView_FieldFilterEditorCreated(object sender, Telerik.Windows.Controls.GridView.EditorCreatedEventArgs e)
        {
            if (e.Editor is StringFilterEditor stringFilterEditor)
            {
                stringFilterEditor.MatchCaseVisibility = Visibility.Collapsed;
            }
        }
    }
}


