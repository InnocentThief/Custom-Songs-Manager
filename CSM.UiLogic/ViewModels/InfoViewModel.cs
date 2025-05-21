using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Commands;
using System.Diagnostics;
using System.Reflection;

namespace CSM.UiLogic.ViewModels
{
    internal class InfoViewModel(
        IServiceLocator serviceLocator)
        : BaseEditViewModel(serviceLocator, string.Empty, EditViewModelCommandColor.Default, "OK", EditViewModelCommandColor.Default)
    {
        private IRelayCommand? openGithubCommand;

        public override string Title => "About Custom Songs Manager";

        public static string Version => $"Version {Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString()}";

        public IRelayCommand OpenGithubCommand => openGithubCommand ??= CommandFactory.Create(OpenGithub, CanOpenGithub);


        private void OpenGithub()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/InnocentThief/Custom-Songs-Manager",
                UseShellExecute = true
            });
        }

        private bool CanOpenGithub()
        {
            return true;
        }
    }
}
