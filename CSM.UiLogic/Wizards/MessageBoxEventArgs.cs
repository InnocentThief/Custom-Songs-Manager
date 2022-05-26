using System;

namespace CSM.UiLogic.Wizards
{
    /// <summary>
    /// EventArgs used for message boxes.
    /// </summary>
    public class MessageBoxEventArgs : EventArgs
    {
        /// <summary>
        /// The view model for the message box to show.
        /// </summary>
        public MessageBoxViewModel MessageBoxViewModel { get; }

        /// <summary>
        /// Initializes a new <see cref="MessageBoxEventArgs"/>.
        /// </summary>
        /// <param name="messageBoxViewModel">ViewModel containing the information for the message box.</param>
        public MessageBoxEventArgs(MessageBoxViewModel messageBoxViewModel)
        {
            MessageBoxViewModel = messageBoxViewModel;
        }
    }
}