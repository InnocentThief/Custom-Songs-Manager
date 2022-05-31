using System;

namespace CSM.UiLogic.Wizards
{
    /// <summary>
    /// EventArgs used for edit windows.
    /// </summary>
    public class EditWindowEventArgs : EventArgs
    {
        /// <summary>
        /// The view model for the edit window.
        /// </summary>
        public EditWindowBaseViewModel EditWindowViewModel { get; }

        /// <summary>
        /// Initializes a new <see cref="EditWindowEventArgs"/>.
        /// </summary>
        /// <param name="editWindowViewModel">ViewModel containing the information for the edit window.</param>
        public EditWindowEventArgs(EditWindowBaseViewModel editWindowViewModel)
        {
            EditWindowViewModel = editWindowViewModel;
        }
    }
}