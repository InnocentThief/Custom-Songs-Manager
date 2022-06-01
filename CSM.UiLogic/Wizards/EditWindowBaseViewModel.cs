using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;

namespace CSM.UiLogic.Wizards
{
    /// <summary>
    /// Basic view model respresenting the data for a edit wizard window.
    /// </summary>
    public abstract class EditWindowBaseViewModel : ObservableObject
    {
        #region Public Properties

        /// <summary>
        /// Title of the edit wizard (shown in the title bar of the dialog).
        /// </summary>
        public abstract string Title { get; }

        /// <summary>
        /// Gets the command used to cancel the action.
        /// </summary>
        public RelayCommand CancelCommand { get; }

        /// <summary>
        /// Gets the text for the cancel button.
        /// </summary>
        public string CancelCommandText { get; }

        /// <summary>
        /// Gets whether the cancel button is visible.
        /// </summary>
        public bool CancelCommandVisible { get; }

        /// <summary>
        /// Gets the command used to continue with the action.
        /// </summary>
        public RelayCommand ContinueCommand { get; }

        /// <summary>
        /// Gets the text for the continue button.
        /// </summary>
        public string ContinueCommandText { get; }

        /// <summary>
        /// Gets whether the continue button is visible.
        /// </summary>
        public bool ContinueCommandVisible { get; }

        /// <summary>
        /// Gets the height of the window.
        /// </summary>
        public abstract int Height { get; }

        /// <summary>
        /// Gets the width of the window.
        /// </summary>
        public abstract int Width { get; }

        /// <summary>
        /// Gets whether the user wants to continue with the action.
        /// </summary>
        public bool Continue { get; private set; }

        #endregion

        /// <summary>
        /// Initializes a new <see cref="EditWindowBaseViewModel"/>.
        /// </summary>
        /// <param name="continueCommandText">The text used for the continue button.</param>
        /// <param name="cancelCommandText">The text used for the cancel button.</param>
        protected EditWindowBaseViewModel(string continueCommandText, string cancelCommandText)
        {
            ContinueCommand = new RelayCommand(ContinueAction, CanContinue);
            ContinueCommandText = continueCommandText;
            ContinueCommandVisible = !string.IsNullOrWhiteSpace(continueCommandText);
            CancelCommand = new RelayCommand(CancelAction, CanCancel);
            CancelCommandText = cancelCommandText;
            CancelCommandVisible = !string.IsNullOrWhiteSpace(cancelCommandText);
        }

        /// <summary>
        /// Action to be executed when to close the dialog.
        /// </summary>
        public Action CloseAction { get; set; }

        #region Helper methods

        private void CancelAction()
        {
            Continue = false;
            CloseAction();
        }

        protected virtual bool CanCancel()
        {
            return true;
        }

        public void ContinueAction()
        {
            Continue = true;
            CloseAction();
        }

        protected virtual bool CanContinue()
        {
            return true;
        }

        #endregion
    }
}