using CSM.UiLogic.ViewModels.Common.Playlists;
using System.Windows.Controls;

namespace CSM.App.Views.Controls.SongSources
{
    /// <summary>
    /// Interaction logic for SongSourcesSongSuggestControl.xaml
    /// </summary>
    public partial class SongSourcesSongSuggestControl : UserControl
    {
        public SongSourcesSongSuggestControl()
        {
            InitializeComponent();
        }

        private async void RadGridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            if (sender is Telerik.Windows.Controls.RadGridView gridView && gridView.DataContext is PlaylistViewModel viewModel)
            {
                await viewModel.LoadSelectedSongDataAsync();
            }
        }

        private void RadGridView_FilterOperatorsLoading(object sender, Telerik.Windows.Controls.GridView.FilterOperatorsLoadingEventArgs e)
        {
            e.DefaultOperator1 = Telerik.Windows.Data.FilterOperator.Contains;
        }
    }
}
