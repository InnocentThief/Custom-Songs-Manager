using CSM.UiLogic.Workspaces.Playlists;
using System.Windows.Controls;

namespace CSM.App.Workspaces.Playlists
{
    /// <summary>
    /// Interaction logic for SongSearchView.xaml
    /// </summary>
    public partial class SongSearchView : UserControl
    {
        public SongSearchView()
        {
            InitializeComponent();
        }

        private void RadWatermarkTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            var viewModel = DataContext as SongSearchViewModel;
            viewModel.Search();
        }
    }
}