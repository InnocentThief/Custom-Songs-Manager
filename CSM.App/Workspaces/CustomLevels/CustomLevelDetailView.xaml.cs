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