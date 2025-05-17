using System.Windows.Controls;
using CSM.UiLogic.ViewModels.Controls.BeatLeader;
using CSM.UiLogic.ViewModels.Controls.SongSources;

namespace CSM.App.Views.Controls.SongSources
{
    /// <summary>
    /// Interaction logic for SongSourcesControl.xaml
    /// </summary>
    public partial class SongSourcesControl : UserControl
    {
        public SongSourcesControl()
        {
            InitializeComponent();
        }

        private async void CustomLevels_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is SongSourcesControlViewModel viewModel)
            {
                await viewModel.LoadAsync();
            }
        }

        private async void Playlists_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is SongSourcesControlViewModel viewModel)
            {
                await viewModel.LoadAsync();
            }
        }

        private async void Favourites_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is SongSourcesControlViewModel viewModel)
            {
                await viewModel.LoadAsync();
            }
        }

        private async void SongSearch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is SongSourcesControlViewModel viewModel)
            {
                await viewModel.LoadAsync();
            }
        }

        private async void SongSuggest_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is SongSourcesControlViewModel viewModel)
            {
                await viewModel.LoadAsync();
            }
        }

        private async void Twitch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is SongSourcesControlViewModel viewModel)
            {
                await viewModel.LoadAsync();
            }
        }

        private async void BeatLeader_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is SongSourcesControlViewModel viewModel)
            {
                await viewModel.LoadAsync();
            }
        }
    }
}
