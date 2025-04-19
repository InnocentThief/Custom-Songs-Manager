using System.Windows.Controls;
using CSM.UiLogic.ViewModels.Controls.CustomLevels;

namespace CSM.App.Views.Controls.SongSources
{
    /// <summary>
    /// Interaction logic for SongSourcesCustomLevelsControl.xaml
    /// </summary>
    public partial class SongSourcesCustomLevelsControl : UserControl
    {
        public SongSourcesCustomLevelsControl()
        {
            InitializeComponent();
        }

        private async void RadGridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (DataContext is CustomLevelsControlViewModel viewModel)
            {
                await viewModel.LoadSelectedCustomLevelDataAsync();
            }
        }
    }
}
