using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Common.Leaderboard
{
    internal abstract class BasePlayerViewModel : BaseViewModel
    {
        public abstract string Name { get; }

        public abstract string Avatar { get; }

        public abstract double PP { get; } 

        public abstract int Rank { get; }

        public abstract string Country { get; }

        public abstract int CountryRank { get; }

        protected BasePlayerViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }
    }
}
