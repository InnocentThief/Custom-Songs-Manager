using CSM.UiLogic.ViewModels.Controls.ScoreSaber;
using System.Windows.Controls;
using System.Windows.Input;

namespace CSM.App.Views.Controls.ScoreSaber
{
    /// <summary>
    /// Interaction logic for ScoreSaberControl.xaml
    /// </summary>
    public partial class ScoreSaberControl : UserControl
    {
        public ScoreSaberControl()
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
