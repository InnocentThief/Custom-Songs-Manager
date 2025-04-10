namespace CSM.UiLogic.Commands
{
   internal abstract class BaseDelegateCommand : IRelayCommand
    {
        private readonly Func<object?, Task> executeMethod;
        private readonly Predicate<object?> canExecuteMethod;

        public event EventHandler? CanExecuteChanged;

        protected BaseDelegateCommand(Action<object?> executeMethod, Predicate<object?> canExecuteMethod)
        {
            this.executeMethod = arg =>
            {
                executeMethod(arg);
                return Task.CompletedTask;
            };
            this.canExecuteMethod = canExecuteMethod;
        }

        protected BaseDelegateCommand(Func<object?, Task> executeMethod, Predicate<object?> canExecuteMethod)
        {
            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        protected Task ExecuteAsync(object? parameter)
        {
            return executeMethod(parameter);
        }

        public bool CanExecute(object? parameter)
        {
            return canExecuteMethod == null || canExecuteMethod(parameter);
        }

        public async void Execute(object? parameter)
        {
            await ExecuteAsync(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }
    }
}
