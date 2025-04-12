using CSM.Framework.ServiceLocation;
using CSM.UiLogic.Commands;
using CSM.UiLogic.Services;

namespace CSM.UiLogic.AbstractBase
{
    internal abstract class BaseViewModel : BaseNotifiable
    {
        private bool loadingInProgress;

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

        protected ICommandFactory CommandFactory { get; }

        protected IServiceLocator ServiceLocator { get; }

        protected IUserInteraction UserInteraction { get; }

        protected BaseViewModel(IServiceLocator serviceLocator)
        {
            ServiceLocator = serviceLocator;
            CommandFactory = ServiceLocator.GetService<ICommandFactory>();
            UserInteraction = ServiceLocator.GetService<IUserInteraction>();
        }
    }
}
