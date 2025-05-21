using CSM.App.Views.Windows;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Services;
using System.Windows;

namespace CSM.App.Services
{
    internal class UserInteraction(IServiceLocator serviceLocator) : IUserInteraction
    {
        private readonly IServiceLocator serviceLocator = serviceLocator;

        public void ShowError(string message)
        {
            //throw new NotImplementedException();
        }

        public void ShowWarning(string message)
        {
            throw new NotImplementedException();
        }

        public void ShowWindow<T>(T value) where T : BaseEditViewModel
        {
            var window = serviceLocator.GetService<EditWindow>();
            window.ViewModel = value;
            window.Owner = Application.Current.MainWindow;
            window.ShowDialog();
        }
    }
}
