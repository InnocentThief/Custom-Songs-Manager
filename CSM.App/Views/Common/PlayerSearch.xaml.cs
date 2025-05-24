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
using CSM.UiLogic.ViewModels.Controls.ScoreSaber;

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
