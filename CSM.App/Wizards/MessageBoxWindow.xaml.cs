using System.Windows;
using Telerik.Windows.Controls;

namespace CSM.App.Wizards
{
    /// <summary>
    /// Interaction logic for MessageBoxWindow.xaml
    /// </summary>
    public partial class MessageBoxWindow : RadWindow
    {
        public MessageBoxWindow()
        {
            InitializeComponent();

            IconTemplate = this.Resources["WindowIconTemplate"] as DataTemplate;
        }
    }
}