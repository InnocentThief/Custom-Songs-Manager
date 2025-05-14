using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Common.Leaderboard
{
    internal abstract class BaseScoreViewModel : BaseViewModel
    {
        public abstract int Rank { get; }

        public abstract string Id { get; }

        public abstract string SongName { get; }

        public abstract string SubName { get; }

        public abstract string Author { get; }

        public abstract string Mapper { get; }

        public abstract string CoverImage { get; }

        public abstract bool FC { get; }

        public abstract int BadCuts { get; }

        public abstract int MissedNotes { get; }

        protected BaseScoreViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }
    }
}
