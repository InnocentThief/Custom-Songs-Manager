using System.Windows.Controls;
using CSM.UiLogic.ViewModels.Controls.Settings;

namespace CSM.App.Views.Windows.Settings
{
    /// <summary>
    /// Interaction logic for CustomLevelsSettingsControl.xaml
    /// </summary>
    public partial class CustomLevelsSettingsControl : UserControl
    {
        public CustomLevelsSettingsControl()
        {
            InitializeComponent();
        }

        private void RadFilePathPicker_FilePathChanged(object sender, Telerik.Windows.Controls.FileDialogs.FilePathChangedEventArgs e)
        {
            if (DataContext is CustomLevelsSettingsViewModel viewModel)
            {
                viewModel.CustomLevelsPath = e.FilePath;
            }
        }
    }
}
