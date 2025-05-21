using CSM.UiLogic.ViewModels.Controls.BeatLeader;
using System.Windows.Controls;
using System.Windows.Input;

namespace CSM.App.Views.Controls.BeatLeader
{
    /// <summary>
    /// Interaction logic for BeatLeaderControl.xaml
    /// </summary>
    public partial class BeatLeaderControl : UserControl
    {
        public BeatLeaderControl()
        {
            InitializeComponent();
        }

        private async void RadWatermarkTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            if (DataContext is BeatLeaderControlViewModel viewModel)
            {
                await viewModel.PlayerSearch.SearchAsync();
            }
        }
    }
}
