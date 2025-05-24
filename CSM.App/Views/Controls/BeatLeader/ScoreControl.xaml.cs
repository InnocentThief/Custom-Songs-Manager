using CSM.App.Views.Helper;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.ViewModels.Controls.CustomLevels;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls.Filtering.Editors;
using Telerik.Windows.Persistence;

namespace CSM.App.Views.Controls.BeatLeader
{
    /// <summary>
    /// Interaction logic for ScoreControl.xaml
    /// </summary>
    public partial class ScoreControl : UserControl
    {
        private PersistenceManager persistenceManager = PersistenceFrameworkHelper.GetPersistenceManager();

        public ScoreControl()
        {
            InitializeComponent();
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CustomLevelsControlViewModel viewModel)
            {
                var stream = persistenceManager.Save(BlScoresGridView);
                await viewModel.SaveViewDefinitionAsync(stream, SavableUiElement.BlMainControl);
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CustomLevelsControlViewModel viewModel && viewModel.SelectedViewDefinition != null)
            {
                var stream = persistenceManager.Save(BlScoresGridView);
                await viewModel.SaveViewDefinitionAsync(stream, SavableUiElement.BlMainControl, viewModel.SelectedViewDefinition.Name);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CustomLevelsControlViewModel viewModel && viewModel.SelectedViewDefinition != null)
            {
                viewModel.DeleteViewDefinition(SavableUiElement.BlMainControl, viewModel.SelectedViewDefinition.Name);
            }
        }

        private void ViewDefinitions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is CustomLevelsControlViewModel viewModel)
            {
                if (viewModel.SelectedViewDefinition != null && viewModel.SelectedViewDefinition.Stream != null)
                {
                    persistenceManager.Load(BlScoresGridView, viewModel.SelectedViewDefinition.Stream);
                    viewModel.SelectedViewDefinition.Stream.Position = 0;
                }
            }
        }

        private void BlScoresGridView_FieldFilterEditorCreated(object sender, Telerik.Windows.Controls.GridView.EditorCreatedEventArgs e)
        {
            if (e.Editor is StringFilterEditor stringFilterEditor)
            {
                stringFilterEditor.MatchCaseVisibility = Visibility.Collapsed;
            }
        }
    }
}
