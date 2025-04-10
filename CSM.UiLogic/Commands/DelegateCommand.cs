namespace CSM.UiLogic.Commands
{
    internal sealed class DelegateCommand : BaseDelegateCommand
    {
        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod) : base(o => executeMethod(), o => canExecuteMethod())
        {
        }

        private DelegateCommand(Func<Task> executeMethod, Func<bool> canExecuteMethod) : base(o => executeMethod(), o => canExecuteMethod())
        {
        }

        public Task ExecuteAsync()
        {
            return ExecuteAsync(null);
        }

        public static DelegateCommand FromAsyncHandler(Func<Task> executeMethod, Func<bool> canExecuteMethod)
        {
            return new DelegateCommand(executeMethod, canExecuteMethod);
        }
    }
}
