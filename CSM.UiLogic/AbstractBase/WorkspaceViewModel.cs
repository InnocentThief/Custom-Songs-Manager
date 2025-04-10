using CSM.Framework.ServiceLocation;

namespace CSM.UiLogic.AbstractBase
{
    internal abstract class WorkspaceViewModel : BaseViewModel
    {
        public abstract string Title { get; }

        public bool IsActive { get; set; }

        protected WorkspaceViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public abstract Task ActivateAsync(bool refresh = false);
    }
}
