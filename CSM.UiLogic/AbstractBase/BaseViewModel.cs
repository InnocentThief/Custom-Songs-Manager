using CSM.Framework.ServiceLocation;
using CSM.UiLogic.Commands;

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

        protected BaseViewModel(IServiceLocator serviceLocator)
        {
            ServiceLocator = serviceLocator;
            CommandFactory = ServiceLocator.GetService<ICommandFactory>();
        }
    }
}
