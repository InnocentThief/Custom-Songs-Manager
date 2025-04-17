using CSM.Framework.ServiceLocation;
using CSM.UiLogic.Commands;
using CSM.UiLogic.Services;

namespace CSM.UiLogic.AbstractBase
{
    internal abstract class BaseViewModel : BaseNotifiable
    {
        private bool loadingInProgress;
        private string loadingInProgressMessage = string.Empty;

        public bool LoadingInProgress
        {
            get => loadingInProgress;
            set
            {
                if (loadingInProgress == value)
                    return;
                loadingInProgress = value;
                OnPropertyChanged();
            }
        }

        public string LoadingInProgressMessage
        {
            get => loadingInProgressMessage;
            set
            {
                if (loadingInProgressMessage == value)
                    return;
                loadingInProgressMessage = value;
                OnPropertyChanged();
            }
        }

        protected ICommandFactory CommandFactory { get; }

        protected IServiceLocator ServiceLocator { get; }

        protected IUserInteraction UserInteraction { get; }

        protected IUiText UiText { get; }

        protected BaseViewModel(IServiceLocator serviceLocator)
        {
            ServiceLocator = serviceLocator;
            CommandFactory = ServiceLocator.GetService<ICommandFactory>();
            UserInteraction = ServiceLocator.GetService<IUserInteraction>();
            UiText = ServiceLocator.GetService<IUiText>();
        }

        protected void SetLoadingInProgress(bool value, string message)
        {
            LoadingInProgressMessage = message;
            LoadingInProgress = value;
        }
    }
}
