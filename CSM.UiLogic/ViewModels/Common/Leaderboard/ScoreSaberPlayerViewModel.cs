using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSM.Framework.ServiceLocation;

namespace CSM.UiLogic.ViewModels.Common.Leaderboard
{
    internal sealed class ScoreSaberPlayerViewModel : BasePlayerViewModel
    {
        public override string Name => throw new NotImplementedException();

        public override string Avatar => throw new NotImplementedException();

        public override double PP => throw new NotImplementedException();

        public override int Rank => throw new NotImplementedException();

        public override string Country => throw new NotImplementedException();

        public override int CountryRank => throw new NotImplementedException();

        public ScoreSaberPlayerViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }
    }
}
