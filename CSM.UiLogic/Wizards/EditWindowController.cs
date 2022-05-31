using System;

namespace CSM.UiLogic.Wizards
{
    /// <summary>
    /// Handles edit window events.
    /// </summary>
    public class EditWindowController
    {
        /// <summary>
        /// Occurs when an edit window should be displayed.
        /// </summary>
        public event EventHandler<EditWindowEventArgs> ShowEditWindowEvent;

        /// <summary>
        /// Initializes a new <see cref="EditWindowController"/>.
        /// </summary>
        private EditWindowController()
        {

        }

        /// <summary>
        /// Shows the edit window for the given view model.
        /// </summary>
        /// <param name="editWindowViewModel"></param>
        public void ShowEditWindow(EditWindowBaseViewModel editWindowViewModel)
        {
            var eventArgs = new EditWindowEventArgs(editWindowViewModel);
            ShowEditWindowEvent?.Invoke(this, eventArgs);
        }

        #region Singleton

        private static EditWindowController instance;

        public static EditWindowController Instance()
        {
            if (instance == null)
            {
                instance = new EditWindowController();
            }
            return instance;
        }

        #endregion
    }
}