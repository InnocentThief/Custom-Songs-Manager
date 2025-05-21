using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Common.Leaderboard
{
    internal abstract class BasePlayerViewModel : BaseViewModel
    {
        public abstract string Id { get; }

        public abstract string Name { get; }

        public abstract string Avatar { get; }

        public abstract string PP { get; }

        public abstract string Rank { get; }

        public abstract string Country { get; }

        public abstract string CountryRank { get; }

        protected BasePlayerViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }
    }
}
