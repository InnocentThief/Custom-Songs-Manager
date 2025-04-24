using CSM.UiLogic.ViewModels.Common.Playlists;
using System.Windows.Controls;

namespace CSM.App.Views.Controls.Playlists
{
    /// <summary>
    /// Interaction logic for PlaylistDataTemplate.xaml
    /// </summary>
    public partial class PlaylistDataTemplate : UserControl
    {
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
    }
}
