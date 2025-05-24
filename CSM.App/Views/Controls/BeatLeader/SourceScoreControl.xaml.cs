using System.Windows;
using System.Windows.Controls;
using CSM.App.Views.Helper;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.ViewModels.Controls.BeatLeader;
using Telerik.Windows.Controls.Filtering.Editors;
using Telerik.Windows.Persistence;

namespace CSM.App.Views.Controls.BeatLeader
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
            if (DataContext is BeatLeaderControlViewModel viewModel)
            {
                var stream = persistenceManager.Save(BlSourceScoresGridView);
                await viewModel.SaveViewDefinitionAsync(stream, SavableUiElement.BlSourceControl);
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is BeatLeaderControlViewModel viewModel && viewModel.SelectedViewDefinition != null)
            {
                var stream = persistenceManager.Save(BlSourceScoresGridView);
                await viewModel.SaveViewDefinitionAsync(stream, SavableUiElement.BlSourceControl, viewModel.SelectedViewDefinition.Name);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is BeatLeaderControlViewModel viewModel && viewModel.SelectedViewDefinition != null)
            {
                viewModel.DeleteViewDefinition(SavableUiElement.BlSourceControl, viewModel.SelectedViewDefinition.Name);
            }
        }

        private void ViewDefinitions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is BeatLeaderControlViewModel viewModel)
            {
                if (viewModel.SelectedViewDefinition != null && viewModel.SelectedViewDefinition.Stream != null)
                {
                    persistenceManager.Load(BlSourceScoresGridView, viewModel.SelectedViewDefinition.Stream);
                    viewModel.SelectedViewDefinition.Stream.Position = 0;
                }
            }
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
