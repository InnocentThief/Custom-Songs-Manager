using CSM.DataAccess.Entities.Types;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Windows.Media;

namespace CSM.UiLogic.Wizards
{
    /// <summary>
    /// ViewModel representing the data for a message box.
    /// </summary>
    public class MessageBoxViewModel
    {
        #region Private fields

        private SolidColorBrush defaultButtonColor = new SolidColorBrush(Colors.Transparent);
        private SolidColorBrush attentionButtonColor = new SolidColorBrush(Colors.DarkRed);

        private MessageBoxButtonColor firstCommandMessageButtonColor;
        private MessageBoxButtonColor secondCommandMessageButtonColor;

        #endregion

        #region Public Properties

        /// <summary>
        /// Title of the message box (shown in the title bar of the dialog).
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Message of the message box (shown as content of the dialog).
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets the type of the message box.
        /// </summary>
        public MessageBoxTypes MessageBoxType { get; set; }

        /// <summary>
        /// Command used for the first available action.
        /// </summary>
        public RelayCommand FirstCommand { get; }

        /// <summary>
        /// Gets the text for the first command.
        /// </summary>
        public string FirstCommandText { get; }

        /// <summary>
        /// Gets whether the first command is visible.
        /// </summary>
        public bool FirstCommandVisible { get; }

        /// <summary>
        /// Gets the background color for the first command.
        /// </summary>
        public SolidColorBrush FirstCommandColor
        {
            get
            {
                switch (firstCommandMessageButtonColor)
                {
                    case MessageBoxButtonColor.Default:
                        return defaultButtonColor;
                    case MessageBoxButtonColor.Attention:
                        return attentionButtonColor;
                    default:
                        return defaultButtonColor;
                }
            }
        }

        /// <summary>
        /// Command used for the second available action.
        /// </summary>
        public RelayCommand SecondCommand { get; }

        /// <summary>
        /// Gets the text for the second command.
        /// </summary>
        public string SecondCommandText { get; }

        /// <summary>
        /// Gets whether the second command is visible.
        /// </summary>
        public bool SecondCommandVisible { get; }

        /// <summary>
        /// Gets the background color for the second command.
        /// </summary>
        public SolidColorBrush SecondCommandColor
        {
            get
            {
                switch (secondCommandMessageButtonColor)
                {
                    case MessageBoxButtonColor.Default:
                        return defaultButtonColor;
                    case MessageBoxButtonColor.Attention:
                        return attentionButtonColor;
                    default:
                        return defaultButtonColor;
                }
            }
        }

        /// <summary>
        /// Gets whether the natification icon is visible.
        /// </summary>
        public bool NotificationVisible => MessageBoxType == MessageBoxTypes.Notification;

        /// <summary>
        /// Gets whether the information icon is visible.
        /// </summary>
        public bool InformationVisible => MessageBoxType == MessageBoxTypes.Information;

        /// <summary>
        /// Gets whether the question icon is visible.
        /// </summary>
        public bool QuestionVisible => MessageBoxType == MessageBoxTypes.Question;

        /// <summary>
        /// Gets whether the warning icon is visible.
        /// </summary>
        public bool WarningVisible => MessageBoxType == MessageBoxTypes.Warning;

        /// <summary>
        /// Gets whether the user wants to continue with the action.
        /// </summary>
        public bool Continue { get; private set; }

        #endregion

        /// <summary>
        /// Initializes a new <see cref="MessageBoxViewModel"/>.
        /// </summary>
        /// <param name="firstCommandText">Text for the first command.</param>
        /// <param name="firstCommandColor">Color for the first command.</param>
        /// <param name="secondCommandText">Text for the second command.</param>
        /// <param name="secondCommandColor">Color for the second command.</param>
        public MessageBoxViewModel(string firstCommandText, MessageBoxButtonColor firstCommandColor, string secondCommandText, MessageBoxButtonColor secondCommandColor)
        {
            FirstCommand = new RelayCommand(DoFirstCommand);
            FirstCommandText = firstCommandText;
            FirstCommandVisible = !string.IsNullOrWhiteSpace(firstCommandText);
            firstCommandMessageButtonColor = firstCommandColor;
            SecondCommand = new RelayCommand(DoSecondCommand);
            SecondCommandText = secondCommandText;
            SecondCommandVisible = !string.IsNullOrWhiteSpace(secondCommandText);
            secondCommandMessageButtonColor = secondCommandColor;
        }

        /// <summary>
        /// Action to be executed when to close the dialog.
        /// </summary>
        public Action CloseAction { get; set; }

        #region Helper methods

        private void DoFirstCommand()
        {
            Continue = true;
            CloseAction();
        }

        private void DoSecondCommand()
        {
            Continue = false;
            CloseAction();
        }

        #endregion
    }
}