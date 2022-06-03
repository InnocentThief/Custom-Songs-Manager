using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.Tools.CleanupCustomLevels
{
    public class StepStartViewModel : StepBaseViewModel
    {
        public RelayCommand StartCommand { get; }

        public StepStartViewModel() : base("Start", "Informations")
        {
            StartCommand = new RelayCommand(Start);
        }

        private void Start()
        {
            Progress();
        }

        public override async Task LoadDataAsync()
        {
            await Task.CompletedTask;
        }
    }
}
