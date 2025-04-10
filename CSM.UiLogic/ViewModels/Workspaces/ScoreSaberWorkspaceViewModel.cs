using CSM.Framework;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.ViewModels.Workspaces
{
    internal sealed class ScoreSaberWorkspaceViewModel : WorkspaceViewModel
    {
        public override string Title => "Custom Songs Manager - ScoreSaber";

        public ScoreSaberWorkspaceViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override async Task ActivateAsync(bool refresh)
        {
            await Task.CompletedTask;
        }
    }
}
