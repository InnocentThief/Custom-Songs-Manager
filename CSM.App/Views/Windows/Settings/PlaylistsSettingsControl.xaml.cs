using System.Windows.Controls;
using CSM.UiLogic.ViewModels.Controls.Settings;

namespace CSM.App.Views.Windows.Settings
{
    /// <summary>
    /// Interaction logic for PlaylistsSettingsControll.xaml
    /// </summary>
    public partial class PlaylistsSettingsControl : UserControl
    {
        public PlaylistsSettingsControl()
        {
            InitializeComponent();
        }

        private void RadFilePathPicker_FilePathChanged(object sender, Telerik.Windows.Controls.FileDialogs.FilePathChangedEventArgs e)
        {
            if (DataContext is PlaylistsSettingsViewModel viewModel)
            {
                viewModel.PlaylistsPath = e.FilePath;
            }
        }
    }
}
