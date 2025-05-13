using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Common.Leaderboard
{
    internal abstract class BaseScoreViewModel : BaseViewModel
    {
        public abstract int Rank { get; }

        public abstract string SongName { get; }

        public abstract bool FC { get; }

        public abstract int BadCuts { get; }

        public abstract int MissedNotes { get; }

        protected BaseScoreViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }
    }
}
