using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.Tools.CleanupCustomLevels
{
    /// <summary>
    /// Base class for a cleanup custom levels step.
    /// </summary>
    public abstract class StepBaseViewModel: ObservableObject
    {
        public string Text { get; }

        public string AdditionalText { get; }

        public event EventHandler ProgressEvent;

        /// <summary>
        /// Initializes a new <see cref="StepBaseViewModel"/>.
        /// </summary>
        /// <param name="text">The title of the step.</param>
        /// <param name="additionalText">The additional text for the step.</param>
        protected StepBaseViewModel(string text, string additionalText)
        {
            Text = text;
            AdditionalText = additionalText;
        }

        /// <summary>
        /// Loads the step data in asynchronous fashion.
        /// </summary>
        /// <returns>An awaitable task that yields no result.</returns>
        public abstract Task LoadDataAsync();

        /// <summary>
        /// Progresses to the next step.
        /// </summary>
        protected void Progress()
        {
            ProgressEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}