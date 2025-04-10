namespace CSM.UiLogic.Commands
{
    internal interface ICommandFactory
    {
        public IRelayCommand Create(Action executeMethod, Func<bool> canExecuteMethod);

        public IRelayCommand CreateFromAsync(Func<Task> executeMethodAsync, Func<bool> canExecuteMethod);
    }
}
