using CSM.UiLogic.Workspaces.TwitchIntegration;
using System.Windows.Controls;

namespace CSM.App.Workspaces.TwitchIntegration
{
    /// <summary>
    /// Interaction logic for TwitchSongHistoryView.xaml
    /// </summary>
    public partial class TwitchSongHistoryView : UserControl
    {
        public TwitchSongHistoryView()
        {
            InitializeComponent();
        }

        private async void RadGridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            var viewModel = DataContext as TwitchViewModel;
            if (viewModel?.SelectedBeatmap != null)
            {
                await viewModel.GetBeatSaverBeatMapDataAsync(viewModel.SelectedBeatmap.Key);
            }
        }
    }
}