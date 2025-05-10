using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSM.Framework.ServiceLocation;

namespace CSM.UiLogic.ViewModels.Common.Leaderboard
{
    internal sealed class ScoreSaberScoreViewModel : BaseScoreViewModel
    {
        public ScoreSaberScoreViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }
    }
}
