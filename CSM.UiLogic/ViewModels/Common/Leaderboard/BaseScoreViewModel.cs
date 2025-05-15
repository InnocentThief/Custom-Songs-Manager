using CSM.DataAccess.Common;
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
        public abstract double Accuracy { get; }
        public string AccuracyString => $"{Accuracy}%";
        public abstract double PP { get; }
        public abstract double WeightedPP { get; }
        public abstract int Pauses { get; }
        public abstract string Modifiers { get; }
        public abstract DateTime Date { get; }
        public abstract Characteristic Characteristic { get; }
        public abstract Difficulty Difficulty { get; }

        protected BaseScoreViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }
    }
}
