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
using Telerik.Windows.Controls.Filtering.Editors;

namespace CSM.App.Views.Controls.BeatLeader
{
    /// <summary>
    /// Interaction logic for SourceScoreControl.xaml
    /// </summary>
    public partial class SourceScoreControl : UserControl
    {
        public SourceScoreControl()
        {
            InitializeComponent();
        }

        private void BlSourceScoresGridView_FieldFilterEditorCreated(object sender, Telerik.Windows.Controls.GridView.EditorCreatedEventArgs e)
        {
            if (e.Editor is StringFilterEditor stringFilterEditor)
            {
                stringFilterEditor.MatchCaseVisibility = Visibility.Collapsed;
            }
        }
    }
}
