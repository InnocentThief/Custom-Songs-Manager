using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Common.Leaderboard
{
    internal abstract class BaseScoreViewModel : BaseViewModel
    {
        protected BaseScoreViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }
    }
}
