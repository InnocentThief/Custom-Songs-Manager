using CSM.App.Views.Helper;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.ViewModels.Common.Leaderboard;
using CSM.UiLogic.ViewModels.Controls.ScoreSaber;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls.Filtering.Editors;
using Telerik.Windows.Persistence;

namespace CSM.App.Views.Controls.ScoreSaber
{
    /// <summary>
    /// Interaction logic for SourceScoreControl.xaml
    /// </summary>
    public partial class SourceScoreControl : UserControl
    {
        private readonly PersistenceManager persistenceManager = PersistenceFrameworkHelper.GetPersistenceManager();

        public SourceScoreControl()
        {
            InitializeComponent();
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ScoreSaberControlViewModel viewModel)
            {
                var stream = persistenceManager.Save(SsSourceScoresGridView);
                await viewModel.SaveViewDefinitionAsync(stream, SavableUiElement.SSSourceControl);
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ScoreSaberControlViewModel viewModel && viewModel.SelectedViewDefinition != null)
            {
                var stream = persistenceManager.Save(SsSourceScoresGridView);
                await viewModel.SaveViewDefinitionAsync(stream, SavableUiElement.SSSourceControl, viewModel.SelectedViewDefinition.Name);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ScoreSaberControlViewModel viewModel && viewModel.SelectedViewDefinition != null)
            {
                viewModel.DeleteViewDefinition(SavableUiElement.SSSourceControl, viewModel.SelectedViewDefinition.Name);
            }
        }

        private void ViewDefinitions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is ScoreSaberControlViewModel viewModel)
            {
                if (viewModel.SelectedViewDefinition != null && viewModel.SelectedViewDefinition.Stream != null)
                {
                    persistenceManager.Load(SsSourceScoresGridView, viewModel.SelectedViewDefinition.Stream);
                    viewModel.SelectedViewDefinition.Stream.Position = 0;
                }
            }
        }

        private void SsSourceScoresGridView_FieldFilterEditorCreated(object sender, Telerik.Windows.Controls.GridView.EditorCreatedEventArgs e)
        {
            if (e.Editor is StringFilterEditor stringFilterEditor)
            {
                stringFilterEditor.MatchCaseVisibility = Visibility.Collapsed;
            }
        }

        private void SsSourceScoresGridView_Filtered(object sender, Telerik.Windows.Controls.GridView.GridViewFilteredEventArgs e)
        {
            if (DataContext is ScoreSaberControlViewModel viewModel)
            {
                viewModel.ScoresFiltered.Clear();
                foreach (var item in SsSourceScoresGridView.Items)
                {
                    if (item is ScoreSaberScoreViewModel score)
                    {
                        viewModel.ScoresFiltered.Add(score);
                    }
                }
                viewModel.FilterChanged();
            }
        }
    }
}
