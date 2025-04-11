using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.ViewModels.Controls.CustomLevels;

namespace CSM.UiLogic.ViewModels.Workspaces
{
    internal sealed class CustomLevelsWorkspaceViewModel : WorkspaceViewModel
    {
        public override string Title => "Custom Songs Manager - Custom Levels";

        public CustomLevelsControlViewModel CustomLevelsControl { get; }

        public CustomLevelsWorkspaceViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
            CustomLevelsControl = new CustomLevelsControlViewModel(ServiceLocator);
        }

        public override async Task ActivateAsync(bool refresh)
        {
            await CustomLevelsControl.LoadAsync(refresh);
        }
    }
}
