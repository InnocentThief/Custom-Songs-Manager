using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls.Filtering.Editors;

namespace CSM.App.Views.Controls.ScoreSaber
{
    /// <summary>
    /// Interaction logic for ScoreControl.xaml
    /// </summary>
    public partial class ScoreControl : UserControl
    {
        public ScoreControl()
        {
            InitializeComponent();
        }

        private void SsScoresGridView_FieldFilterEditorCreated(object sender, Telerik.Windows.Controls.GridView.EditorCreatedEventArgs e)
        {
            if (e.Editor is StringFilterEditor stringFilterEditor)
            {
                stringFilterEditor.MatchCaseVisibility = Visibility.Collapsed;
            }
        }
    }
}