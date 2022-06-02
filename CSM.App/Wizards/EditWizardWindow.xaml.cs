using System.Windows;
using Telerik.Windows.Controls;

namespace CSM.App.Wizards
{
    /// <summary>
    /// Interaction logic for EditWizardWindow.xaml
    /// </summary>
    public partial class EditWizardWindow : RadWindow
    {
        public EditWizardWindow()
        {
            InitializeComponent();

            IconTemplate = this.Resources["WindowIconTemplate"] as DataTemplate;
        }
    }
}