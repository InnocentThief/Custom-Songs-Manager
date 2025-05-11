using CSM.Framework.ServiceLocation;

namespace CSM.UiLogic.ViewModels.Common.Leaderboard
{
    internal sealed class ScoreSaberPlayerViewModel : BasePlayerViewModel
    {
        public override string Id => throw new NotImplementedException();

        public override string Name => throw new NotImplementedException();

        public override string Avatar => throw new NotImplementedException();

        public override string PP => throw new NotImplementedException();

        public override string Rank => throw new NotImplementedException();

        public override string Country => throw new NotImplementedException();

        public override string CountryRank => throw new NotImplementedException();

        public ScoreSaberPlayerViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }
    }
}
