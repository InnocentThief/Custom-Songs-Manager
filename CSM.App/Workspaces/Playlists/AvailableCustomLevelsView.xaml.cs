using CSM.UiLogic.Workspaces.Playlists;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace CSM.App.Workspaces.Playlists
{
    /// <summary>
    /// Interaction logic for AvailableCustomLevelsView.xaml
    /// </summary>
    public partial class AvailableCustomLevelsView : UserControl
    {
        public AvailableCustomLevelsView()
        {
            InitializeComponent();
        }

        private async void RadGridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            var viewModel = DataContext as PlaylistCustomLevelsViewModel;
            if (viewModel?.SelectedCustomLevel != null)
            {
                await viewModel.GetBeatSaverBeatMapDataAsync(viewModel.SelectedCustomLevel.BsrKey);
            }
        }

        private void RadGridView_FilterOperatorsLoading(object sender, Telerik.Windows.Controls.GridView.FilterOperatorsLoadingEventArgs e)
        {
            e.DefaultOperator1 = Telerik.Windows.Data.FilterOperator.Contains;
        }

        private async void RadTabControl_SelectionChanged(object sender, Telerik.Windows.Controls.RadSelectionChangedEventArgs e)
        {


            if (e.AddedItems.Count == 0) return;
            if ((e.AddedItems[0] as RadTabItem).Name == "Favorites")
            {
                if (DataContext is PlaylistCustomLevelsViewModel viewModel) await viewModel.LoadFavoritesAsync(false);
            }
        }

        private async void Favorites_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            var viewModel = DataContext as PlaylistCustomLevelsViewModel;
            if (viewModel?.SelectedFavorite != null)
            {
                await viewModel.GetBeatSaverBeatMapDataAsync(viewModel.SelectedFavorite.Key);
            }
        }

        private void Favorites_FilterOperatorsLoading(object sender, Telerik.Windows.Controls.GridView.FilterOperatorsLoadingEventArgs e)
        {
            e.DefaultOperator1 = Telerik.Windows.Data.FilterOperator.Contains;
        }

        private void SearchedSongs_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            var viewModel = DataContext as PlaylistCustomLevelsViewModel;
            if (viewModel.SelectedSearchedSong != null)
            {
                viewModel.ShowSongDetailForSelectedSong();
            }
        }

        private void SearchedSongs_FilterOperatorsLoading(object sender, Telerik.Windows.Controls.GridView.FilterOperatorsLoadingEventArgs e)
        {
            e.DefaultOperator1 = Telerik.Windows.Data.FilterOperator.Contains;
        }
    }
}