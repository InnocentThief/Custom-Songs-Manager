using CSM.UiLogic.ViewModels.Controls.ScoreSaber;
using System.Windows.Controls;
using System.Windows.Input;

namespace CSM.App.Views.Common
{
    /// <summary>
    /// Interaction logic for PlayerSearch.xaml
    /// </summary>
    public partial class PlayerSearch : UserControl
    {
        public PlayerSearch()
        {
            InitializeComponent();
        }

        private async void RadWatermarkTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            if (DataContext is ScoreSaberControlViewModel viewModel)
            {
                await viewModel.PlayerSearch.SearchAsync();
            }
        }
    }
}
