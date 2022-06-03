using CSM.UiLogic.Wizards;
using Microsoft.Toolkit.Mvvm.Input;
using System.Diagnostics;
using System.Reflection;

namespace CSM.UiLogic.Workspaces.Infos
{
    /// <summary>
    /// ViewModel for the "About Custom Songs Manager" window.
    /// </summary>
    public class EditWindowInfoViewModel : EditWindowBaseViewModel
    {
        /// <summary>
        /// Gets the height of the window.
        /// </summary>
        public override int Height => 200;

        /// <summary>
        /// Gets the width of the window.
        /// </summary>
        public override int Width => 600;

        /// <summary>
        /// Title of the edit wizard (shown in the title bar of the dialog).
        /// </summary>
        public override string Title => "About Custom Songs Manager";

        /// <summary>
        /// Gets the version of the Custom Songs Manager.
        /// </summary>
        public string Version => $"Version {Assembly.GetExecutingAssembly().GetName().Version.ToString()}";

        /// <summary>
        /// Command used to open the Github website.
        /// </summary>
        public RelayCommand GithubCommand { get; }

        /// <summary>
        /// Initializes a new <see cref="EditWindowInfoViewModel"/>.
        /// </summary>
        public EditWindowInfoViewModel() : base(string.Empty, string.Empty)
        {
            GithubCommand = new RelayCommand(OpenGithub);
        }

        private void OpenGithub()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/InnocentThief/Custom-Songs-Manager",
                UseShellExecute = true
            });
        }
    }
}