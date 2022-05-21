using CSM.UiLogic.Workspaces.Playlists;
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
                await viewModel.GetBeatSaverBeatMapDataAsync(viewModel.SelectedPlaylistSong.Hash);
            }
        }

        private void RadGridView_FilterOperatorsLoading(object sender, Telerik.Windows.Controls.GridView.FilterOperatorsLoadingEventArgs e)
        {
            e.DefaultOperator1 = Telerik.Windows.Data.FilterOperator.Contains;
        }
    }
}
