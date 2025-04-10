namespace CSM.UiLogic.Commands
{
    internal sealed class CommandFactory : ICommandFactory
    {
        public IRelayCommand Create(Action executeMethod, Func<bool> canExecuteMethod)
        {
            return new DelegateCommand(executeMethod, canExecuteMethod);
        }

        public IRelayCommand CreateFromAsync(Func<Task> executeMethodAsync, Func<bool> canExecuteMethod)
        {
            return DelegateCommand.FromAsyncHandler(executeMethodAsync, canExecuteMethod);
        }
    }
}
