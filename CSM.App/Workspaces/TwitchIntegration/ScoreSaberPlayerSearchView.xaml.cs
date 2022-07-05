using CSM.UiLogic.Workspaces.TwitchIntegration.ScoreSaberIntegration;
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

namespace CSM.App.Workspaces.TwitchIntegration
{
    /// <summary>
    /// Interaction logic for ScoreSaberPlayerSearchView.xaml
    /// </summary>
    public partial class ScoreSaberPlayerSearchView : UserControl
    {
        public ScoreSaberPlayerSearchView()
        {
            InitializeComponent();
        }

        private async void RadWatermarkTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            var viewModel = DataContext as ScoreSaberViewModel;
            await viewModel.PlayerSearch.SearchAsync();
        }
    }
}
