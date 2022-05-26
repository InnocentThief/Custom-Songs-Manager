using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.Tools.CleanupCustomLevels
{
    public class CleanupCustomLevelsViewModel : ObservableObject
    {
        private StepBaseViewModel selectedStep;

        public List<StepBaseViewModel> Steps { get; }

        public StepBaseViewModel SelectedStep
        {
            get => selectedStep;
            set
            {
                if (value == selectedStep) return;
                selectedStep = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the index of the selected step.
        /// </summary>
        public int SelectedIndex { get; set; }

        public CleanupCustomLevelsViewModel()
        {
            Steps = new List<StepBaseViewModel>();

            // Start Step
            var startStep = new StepStartViewModel();
            startStep.ProgressEvent += ProgressStep;
            Steps.Add(startStep);

            // Directory Names Step
            var directoryNamesStep = new StepDirectoryNamesViewModel();
            directoryNamesStep.ProgressEvent += ProgressStep;
            Steps.Add(directoryNamesStep);

            // Duplicates Step
            var duplicatesStep = new StepDuplicatesViewModel();
            duplicatesStep.ProgressEvent += ProgressStep;
            Steps.Add(duplicatesStep);

            // Versions Step
            var versionStep = new StepVersionsViewModel();
            versionStep.ProgressEvent += ProgressStep;
            Steps.Add(versionStep);

            SelectedStep = Steps.FirstOrDefault();
        }

        private async void ProgressStep(object sender, EventArgs e)
        {
            SelectedIndex++;
            SelectedStep = Steps[SelectedIndex];
            await SelectedStep.LoadDataAsync();
        }
    }
}