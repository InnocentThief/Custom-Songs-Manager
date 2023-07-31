using CSM.UiLogic.Workspaces.ScoreSaberIntegration;
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
using Telerik.Windows.Controls;

namespace CSM.App.Workspaces.ScoreSaberIntegration
{
    /// <summary>
    /// Interaction logic for ScoreSaberSinglePlayerAnalysisView.xaml
    /// </summary>
    public partial class ScoreSaberSinglePlayerAnalysisView : UserControl
    {
        public ScoreSaberSinglePlayerAnalysisView()
        {
            InitializeComponent();
        }

        private void RadGridView_FilterOperatorsLoading(object sender, Telerik.Windows.Controls.GridView.FilterOperatorsLoadingEventArgs e)
        {
            switch (e.Column.Name)
            {
                case "Rank":
                    e.DefaultOperator1 = Telerik.Windows.Data.FilterOperator.IsEqualTo;
                    break;
                case "TimeSet":
                    e.DefaultOperator1 = Telerik.Windows.Data.FilterOperator.IsGreaterThanOrEqualTo;
                    break;
                case "Song":
                    e.DefaultOperator1 = Telerik.Windows.Data.FilterOperator.Contains;
                    break;
                case "ACC":
                    e.DefaultOperator1 = Telerik.Windows.Data.FilterOperator.IsLessThanOrEqualTo;
                    break;
                case "PP":
                    e.DefaultOperator1 = Telerik.Windows.Data.FilterOperator.IsLessThanOrEqualTo;
                    break;
                case "FC":
                    e.DefaultOperator1 = Telerik.Windows.Data.FilterOperator.IsEqualTo;
                    break;
                case "BadCuts":
                    e.DefaultOperator1 = Telerik.Windows.Data.FilterOperator.IsEqualTo;
                    break;
                case "MissedNotes":
                    e.DefaultOperator1 = Telerik.Windows.Data.FilterOperator.IsEqualTo;
                    break;
                case "Modifiers":
                    e.DefaultOperator1 = Telerik.Windows.Data.FilterOperator.Contains;
                    break;
                default:
                    break;
            }
        }

        private void RadGridView_Filtered(object sender, Telerik.Windows.Controls.GridView.GridViewFilteredEventArgs e)
        {
            if (sender is RadGridView gridView)
            {
                if (DataContext is ScoreSaberSinglePlayerAnalysisViewModel viewModel)
                {
                    viewModel.Player.ShowedScores.Clear();
                    foreach (var item in gridView.Items)
                    {
                        if (item is ScoreSaberPlayerScoreViewModel scoreSaberPlayerScore)
                        {
                            viewModel.Player.ShowedScores.Add(scoreSaberPlayerScore);
                        }
                    }
                }
            }
        }
    }
}
