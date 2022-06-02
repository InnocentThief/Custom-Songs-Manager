using System.Windows.Controls;

namespace CSM.App.Workspaces.CustomLevels
{
    /// <summary>
    /// Interaction logic for CustomLevelDetail.xaml
    /// </summary>
    public partial class CustomLevelDetailView : UserControl
    {
        public CustomLevelDetailView()
        {
            InitializeComponent();
        }

        private void RadPropertyGrid_AutoGeneratingPropertyDefinition(object sender, Telerik.Windows.Controls.Data.PropertyGrid.AutoGeneratingPropertyDefinitionEventArgs e)
        {
            if (e.PropertyDefinition.DisplayName == "Difficulties") e.Cancel = true;
        }
    }
}