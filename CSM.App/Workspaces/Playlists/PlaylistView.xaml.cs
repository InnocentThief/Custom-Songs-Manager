using CSM.UiLogic.Workspaces.Playlists;
using System.Windows.Controls;

namespace CSM.App.Workspaces.Playlists
{
    /// <summary>
    /// Interaction logic for PlaylistView.xaml
    /// </summary>
    public partial class PlaylistView : UserControl
    {
        public PlaylistView()
        {
            InitializeComponent();
        }

        private async void RadGridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            var viewModel = DataContext as PlaylistViewModel;
            if (viewModel?.SelectedPlaylistSong != null)
            {
                await viewModel.SetBeatSaverBeatMapDataAsync(viewModel.SelectedPlaylistSong.Hash);
            }
        }

        private void RadGridView_FilterOperatorsLoading(object sender, Telerik.Windows.Controls.GridView.FilterOperatorsLoadingEventArgs e)
        {
            e.DefaultOperator1 = Telerik.Windows.Data.FilterOperator.Contains;
        }

        private void RadGridView_Sorted(object sender, Telerik.Windows.Controls.GridViewSortedEventArgs e)
        {
            var viewModel = DataContext as PlaylistViewModel;
            viewModel.SetSortOrder(e.Column.UniqueName, e.Column.SortingState);
        }
    }
}