using CSM.Services;
using CSM.UiLogic.Wizards;
using CSM.UiLogic.Workspaces.Common;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.TwitchIntegration.ScoreSaberIntegration
{
    public class ScoreSaberViewModel
    {
        private ScoreSaberService scoreSaberService;

        public ObservableCollection<ScoreSaberPlayerViewModel> Players { get; }

        public AsyncRelayCommand AddPlayerCommand { get; }

        public ScoreSaberViewModel()
        {
            Players = new ObservableCollection<ScoreSaberPlayerViewModel>();

            scoreSaberService = new ScoreSaberService();

            AddPlayerCommand = new AsyncRelayCommand(AddPlayer);



            //Players.Add(new ScoreSaberPlayerViewModel());
            //Players.Add(new ScoreSaberPlayerViewModel());
            //Players.Add(new ScoreSaberPlayerViewModel());
            //Players.Add(new ScoreSaberPlayerViewModel());
        }

        private async Task AddPlayer()
        {
            var fileViewModel = new EditWindowNewFileOrFolderNameViewModel("Player Name", "Enter the name of the player which you want to add to the comparison", false);
            EditWindowController.Instance().ShowEditWindow(fileViewModel);
            if (fileViewModel.Continue)
            {
                var player = await scoreSaberService.GetPlayersAsync($"search={fileViewModel.FileOrFolderName}");
            }
        }
    }
}
