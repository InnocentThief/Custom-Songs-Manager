﻿using CSM.UiLogic.Workspaces.TwitchIntegration;
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

namespace CSM.App.Workspaces.TwitchIntegration
{
    /// <summary>
    /// Interaction logic for TwitchSongHistoryView.xaml
    /// </summary>
    public partial class TwitchSongHistoryView : UserControl
    {
        public TwitchSongHistoryView()
        {
            InitializeComponent();
        }

        private async void RadGridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            var viewModel = DataContext as TwitchViewModel;
            if (viewModel?.SelectedBeatmap != null)
            {
                await viewModel.GetBeatSaverBeatMapDataAsync(viewModel.SelectedBeatmap.Key);
            }
        }
    }
}
