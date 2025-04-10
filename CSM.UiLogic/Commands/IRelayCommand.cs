using System.Windows.Input;

namespace CSM.UiLogic.Commands
{
    internal interface IRelayCommand : ICommand
    {
        void RaiseCanExecuteChanged();
    }
}
