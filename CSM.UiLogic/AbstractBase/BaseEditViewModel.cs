using CSM.Framework.ServiceLocation;
using CSM.UiLogic.Commands;
using System.Windows.Media;

namespace CSM.UiLogic.AbstractBase
{
    internal abstract class BaseEditViewModel(
        IServiceLocator serviceLocator,
        string cancelCommandText,
        EditViewModelCommandColor cancelCommandColor,
        string continueCommandText,
        EditViewModelCommandColor continueCommandColor)
        : BaseViewModel(serviceLocator), IBaseEditViewModel
    {
        #region Private fields

        private IRelayCommand? cancelCommand;
        private IRelayCommand? continueCommand;

        private readonly SolidColorBrush defaultCommandColor = new(Colors.Transparent);
        private readonly SolidColorBrush attentionCommandColor = new(Colors.DarkRed);

        private readonly EditViewModelCommandColor cancelCommandColor = cancelCommandColor;
        private readonly EditViewModelCommandColor continueCommandColor = continueCommandColor;

        #endregion

        #region Properties

        public abstract string Title { get; }

        public IRelayCommand CancelCommand => cancelCommand ??= CommandFactory.Create(CancelAction, CanCancel);

        public SolidColorBrush CancelCommandColor
        {
            get
            {
                return cancelCommandColor switch
                {
                    EditViewModelCommandColor.Detault => defaultCommandColor,
                    EditViewModelCommandColor.Attention => attentionCommandColor,
                    _ => defaultCommandColor,
                };
            }
        }

        public string CancelCommandText { get; } = cancelCommandText;

        public IRelayCommand ContinueCommand => continueCommand ??= CommandFactory.Create(ContinueAction, CanContinue);

        public SolidColorBrush ContinueCommandColor
        {
            get
            {
                return continueCommandColor switch
                {
                    EditViewModelCommandColor.Detault => defaultCommandColor,
                    EditViewModelCommandColor.Attention => attentionCommandColor,
                    _ => defaultCommandColor,
                };
            }
        }

        public string ContinueCommandText { get; } = continueCommandText;

        public bool ContinueCommandVisible { get; } = !string.IsNullOrWhiteSpace(continueCommandText);

        public bool Continue { get; private set; }

        #endregion

        public virtual Action? CloseAction { get; set; }

        public event EventHandler? Close;

        private void CancelAction()
        {
            Continue = false;
            Close?.Invoke(this, EventArgs.Empty);
            CloseAction?.Invoke();
        }

        public virtual bool CanCancel()
        {
            return true;
        }

        private void ContinueAction()
        {
            Continue = true;
            Close?.Invoke(this, EventArgs.Empty);
            CloseAction?.Invoke();
        }

        public virtual bool CanContinue()
        {
            return true;
        }
    }
}