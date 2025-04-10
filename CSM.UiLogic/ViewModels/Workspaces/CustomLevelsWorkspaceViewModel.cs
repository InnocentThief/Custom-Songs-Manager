using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.ViewModels.Controls.CustomLevels;

namespace CSM.UiLogic.ViewModels.Workspaces
{
    internal sealed class CustomLevelsWorkspaceViewModel : WorkspaceViewModel
    {
        public override string Title => "Custom Songs Manager - Custom Levels";

        public CustomLevelsControlViewModel CustomLevels { get; }

        public CustomLevelsWorkspaceViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
            CustomLevels = new CustomLevelsControlViewModel(ServiceLocator);
        }

        public override async Task ActivateAsync(bool refresh)
        {
            await CustomLevels.LoadAsync(refresh);
        }
    }
}
