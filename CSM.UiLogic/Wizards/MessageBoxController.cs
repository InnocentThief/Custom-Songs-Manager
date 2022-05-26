using System;

namespace CSM.UiLogic.Wizards
{
    /// <summary>
    /// Handles message box events.
    /// </summary>
    public class MessageBoxController
    {
        /// <summary>
        /// Occurs when a message box should be displayed.
        /// </summary>
        public event EventHandler<MessageBoxEventArgs> ShowMessageBoxEvent;

        /// <summary>
        /// Initializes a new <see cref="MessageBoxController"/>.
        /// </summary>
        private MessageBoxController()
        {

        }

        /// <summary>
        /// Shows a message box for the given view model.
        /// </summary>
        /// <param name="messageBoxViewModel">ViewModel containing the information for the message box.</param>
        public void ShowMessageBox(MessageBoxViewModel messageBoxViewModel)
        {
            var eventArgs = new MessageBoxEventArgs(messageBoxViewModel);
            ShowMessageBoxEvent?.Invoke(this, eventArgs);
        }

        #region Singleton

        private static MessageBoxController instance;

        public static MessageBoxController Instance()
        {
            if (instance == null)
            {
                instance = new MessageBoxController();
            }
            return instance;
        }

        #endregion
    }
}