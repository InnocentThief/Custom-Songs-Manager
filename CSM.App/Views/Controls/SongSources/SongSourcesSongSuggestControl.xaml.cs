using CSM.App.Views.Helper;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.ViewModels.Common.Playlists;
using CSM.UiLogic.ViewModels.Controls.SongSources;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Persistence;

namespace CSM.App.Views.Controls.SongSources
{
    /// <summary>
    /// Interaction logic for SongSourcesSongSuggestControl.xaml
    /// </summary>
    public partial class SongSourcesSongSuggestControl : UserControl
    {
        private PersistenceManager persistenceManager = PersistenceFrameworkHelper.GetPersistenceManager();

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

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is SongSuggestSourceViewModel viewModel && viewModel.Playlist != null)
            {
                var stream = persistenceManager.Save(songSuggestPlaylist);
                await viewModel.Playlist.SaveViewDefinitionAsync(stream, SavableUiElement.SongSuggestPlaylist);
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is SongSuggestSourceViewModel viewModel && viewModel.Playlist != null && viewModel.Playlist.SelectedViewDefinition != null)
            {
                var stream = persistenceManager.Save(songSuggestPlaylist);
                await viewModel.SaveViewDefinitionAsync(stream, SavableUiElement.SongSuggestPlaylist, viewModel.Playlist.SelectedViewDefinition.Name);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is SongSuggestSourceViewModel viewModel && viewModel.Playlist != null && viewModel.Playlist.SelectedViewDefinition != null)
            {
                viewModel.Playlist.DeleteViewDefinition(SavableUiElement.SongSuggestPlaylist, viewModel.Playlist.SelectedViewDefinition.Name);
            }
        }

        private void ViewDefinitions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is SongSuggestSourceViewModel viewModel && viewModel.Playlist != null)
            {
                if (viewModel.Playlist.SelectedViewDefinition != null && viewModel.Playlist.SelectedViewDefinition.Stream != null)
                {
                    persistenceManager.Load(songSuggestPlaylist, viewModel.Playlist.SelectedViewDefinition.Stream);
                    viewModel.Playlist.SelectedViewDefinition.Stream.Position = 0;
                }
            }
        }
    }
}
