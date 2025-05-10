using System.Windows.Controls;
using CSM.UiLogic.ViewModels.Controls.Settings;

namespace CSM.App.Views.Windows.Settings
{
    /// <summary>
    /// Interaction logic for GeneralSettingsControl.xaml
    /// </summary>
    public partial class GeneralSettingsControl : UserControl
    {
        public GeneralSettingsControl()
        {
            InitializeComponent();
        }

        private void RadFilePathPicker_FilePathChanged(object sender, Telerik.Windows.Controls.FileDialogs.FilePathChangedEventArgs e)
        {
            if (DataContext is GeneralSettingsViewModel viewModel)
            {
                viewModel.BeatSaberInstallationPath = e.FilePath;
            }
        }
    }
}
