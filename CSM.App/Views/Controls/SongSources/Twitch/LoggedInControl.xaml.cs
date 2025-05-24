using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls.Filtering.Editors;

namespace CSM.App.Views.Controls.SongSources.Twitch
{
    /// <summary>
    /// Interaction logic for LoggedInControl.xaml
    /// </summary>
    public partial class LoggedInControl : UserControl
    {
        public LoggedInControl()
        {
            InitializeComponent();
        }

        private void TwitchSongHistoryGridView_FieldFilterEditorCreated(object sender, Telerik.Windows.Controls.GridView.EditorCreatedEventArgs e)
        {
            if (e.Editor is StringFilterEditor stringFilterEditor)
            {
                stringFilterEditor.MatchCaseVisibility = Visibility.Collapsed;
            }
        }
    }
}
