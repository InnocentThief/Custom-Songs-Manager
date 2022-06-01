using CSM.UiLogic.Wizards;
using System.IO;

namespace CSM.UiLogic.Workspaces.Common
{
    /// <summary>
    /// ViewModel for new file or folder names handling.
    /// </summary>
    public class EditWindowNewFileOrFolderNameViewModel : EditWindowBaseViewModel
    {
        private string fileOrFolderName;
        private readonly bool folder;

        /// <summary>
        /// Gets the title of the edit wizard window.
        /// </summary>
        public override string Title { get; }

        /// <summary>
        /// Gets the message for the file or folder name.
        /// </summary>
        public string Message { get; }

        /// Gets the height of the window.
        /// </summary>
        public override int Height => 150;

        /// <summary>
        /// Gets the width of the window.
        /// </summary>
        public override int Width => 400;

        /// <summary>
        /// Gets or sets the name of the new file or folder.
        /// </summary>
        public string FileOrFolderName
        {
            get => fileOrFolderName;
            set
            {
                if (value == fileOrFolderName) return;
                fileOrFolderName = value;
                OnPropertyChanged();
                ContinueCommand.NotifyCanExecuteChanged();
            }
        }

        /// <summary>
        /// Initializes a new <see cref="EditWindowNewFileOrFolderNameViewModel"/>.
        /// </summary>
        /// <param name="title">The text used for the window title.</param>
        /// <param name="message">The text used for the file or folder name message.</param>
        /// <param name="folder">Indicates whether it is a folder name of a file name.</param>
        public EditWindowNewFileOrFolderNameViewModel(string title, string message, bool folder) : base("OK", "Cancel")
        {
            Title = title;
            Message = message;
            this.folder = folder;
        }

        protected override bool CanContinue()
        {
            if (string.IsNullOrWhiteSpace(fileOrFolderName)) return false;
            if (folder)
            {
                return fileOrFolderName.IndexOfAny(Path.GetInvalidPathChars()) < 0;
            }
            else
            {
                return fileOrFolderName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
            }
        }
    }
}