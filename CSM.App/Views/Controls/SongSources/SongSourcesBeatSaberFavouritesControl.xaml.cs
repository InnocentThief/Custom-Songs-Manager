using CSM.App.Views.Helper;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.ViewModels.Controls.SongSources;
using CSM.UiLogic.ViewModels.Controls.SongSources.BeatSaberFavourites;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls.Filtering.Editors;
using Telerik.Windows.Persistence;

namespace CSM.App.Views.Controls.SongSources
{
    /// <summary>
    /// Interaction logic for SongSourcesBeatSaberFavouritesControl.xaml
    /// </summary>
    public partial class SongSourcesBeatSaberFavouritesControl : UserControl
    {
        private PersistenceManager persistenceManager = PersistenceFrameworkHelper.GetPersistenceManager();

        public SongSourcesBeatSaberFavouritesControl()
        {
            InitializeComponent();
        }

        private void BeatSaberFavouritesGridView_FilterOperatorsLoading(object sender, Telerik.Windows.Controls.GridView.FilterOperatorsLoadingEventArgs e)
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
            if (DataContext is BeatSaberFavouritesSourceViewModel viewModel)
            {
                var stream = persistenceManager.Save(BeatSaberFavouritesGridView);
                await viewModel.SaveViewDefinitionAsync(stream, SavableUiElement.BeatSaberFavourites);
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is BeatSaberFavouritesSourceViewModel viewModel && viewModel.SelectedViewDefinition != null)
            {
                var stream = persistenceManager.Save(BeatSaberFavouritesGridView);
                await viewModel.SaveViewDefinitionAsync(stream, SavableUiElement.BeatSaberFavourites, viewModel.SelectedViewDefinition.Name);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is BeatSaberFavouritesSourceViewModel viewModel && viewModel.SelectedViewDefinition != null)
            {
                viewModel.DeleteViewDefinition(SavableUiElement.BeatSaberFavourites, viewModel.SelectedViewDefinition.Name);
            }
        }

        private void ViewDefinitions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is BeatSaberFavouritesSourceViewModel viewModel)
            {
                if (viewModel.SelectedViewDefinition != null && viewModel.SelectedViewDefinition.Stream != null)
                {
                    persistenceManager.Load(BeatSaberFavouritesGridView, viewModel.SelectedViewDefinition.Stream);
                    viewModel.SelectedViewDefinition.Stream.Position = 0;
                }
            }
        }

        private void BeatSaberFavouritesGridView_FieldFilterEditorCreated(object sender, Telerik.Windows.Controls.GridView.EditorCreatedEventArgs e)
        {
            if (e.Editor is StringFilterEditor stringFilterEditor)
            {
                stringFilterEditor.MatchCaseVisibility = Visibility.Collapsed;
            }
        }

        private void BeatSaberFavouritesGridView_Filtered(object sender, Telerik.Windows.Controls.GridView.GridViewFilteredEventArgs e)
        {
            if (DataContext is BeatSaberFavouritesSourceViewModel viewModel)
            {
                viewModel.FavouritesFiltered.Clear();
                foreach (var item in BeatSaberFavouritesGridView.Items)
                {
                    if (item is BeatSaberFavouriteViewModel favourite)
                    {
                        viewModel.FavouritesFiltered.Add(favourite);
                    }
                }
                viewModel.FilterChanged();
            }
        }
    }
}
