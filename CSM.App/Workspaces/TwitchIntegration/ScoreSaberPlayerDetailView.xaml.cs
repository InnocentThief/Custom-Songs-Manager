﻿using System;
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

namespace CSM.App.Workspaces.TwitchIntegration
{
    /// <summary>
    /// Interaction logic for ScoreSaberPlayerDetailView.xaml
    /// </summary>
    public partial class ScoreSaberPlayerDetailView : UserControl
    {
        public ScoreSaberPlayerDetailView()
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
                default:
                    break;
            }
        }
    }
}
