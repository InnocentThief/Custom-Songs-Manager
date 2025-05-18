using CSM.App.Views.Helper;
using CSM.Framework.Types;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.ViewModels.Common.Playlists;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Persistence;

namespace CSM.App.Views.Controls.Playlists
{
    /// <summary>
    /// Interaction logic for PlaylistDataTemplate.xaml
    /// </summary>
    public partial class PlaylistDataTemplate : UserControl
    {
        private PersistenceManager persistenceManager = PersistenceFrameworkHelper.GetPersistenceManager();

        public PlaylistDataTemplate()
        {
            InitializeComponent();
        }

        private async void RadGridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            if (DataContext is PlaylistViewModel viewModel)
            {
                await viewModel.LoadSelectedSongDataAsync();
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

        private void RadGridView_Sorted(object sender, Telerik.Windows.Controls.GridViewSortedEventArgs e)
        {
            if (DataContext is PlaylistViewModel viewModel)
            {
                var sortingState = e.Column.SortingState switch
                {
                    Telerik.Windows.Controls.SortingState.None => GridViewSortingState.None,
                    Telerik.Windows.Controls.SortingState.Ascending => GridViewSortingState.Ascending,
                    Telerik.Windows.Controls.SortingState.Descending => GridViewSortingState.Descending,
                    _ => GridViewSortingState.None
                };

                viewModel.SetSortOrder(e.Column.UniqueName, sortingState);
            }
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is PlaylistViewModel viewModel)
            {
                var stream = persistenceManager.Save(playlistLeftGridView);
                await viewModel.SaveViewDefinitionAsync(stream, SavableUiElement.PlaylistLeft);
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is PlaylistViewModel viewModel && viewModel.SelectedViewDefinition != null)
            {
                var stream = persistenceManager.Save(playlistLeftGridView);
                await viewModel.SaveViewDefinitionAsync(stream, SavableUiElement.PlaylistLeft, viewModel.SelectedViewDefinition.Name);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is PlaylistViewModel viewModel && viewModel.SelectedViewDefinition != null)
            {
                viewModel.DeleteViewDefinition(SavableUiElement.PlaylistLeft, viewModel.SelectedViewDefinition.Name);
            }
        }

        private void ViewDefinitions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is PlaylistViewModel viewModel)
            {
                if (viewModel.SelectedViewDefinition != null && viewModel.SelectedViewDefinition.Stream != null)
                {
                    persistenceManager.Load(playlistLeftGridView, viewModel.SelectedViewDefinition.Stream);
                    viewModel.SelectedViewDefinition.Stream.Position = 0;
                }
            }
        }
    }
}
