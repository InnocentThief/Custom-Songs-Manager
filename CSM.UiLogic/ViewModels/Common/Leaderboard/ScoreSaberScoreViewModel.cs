using CSM.Framework.ServiceLocation;

namespace CSM.UiLogic.ViewModels.Common.Leaderboard
{
    internal sealed class ScoreSaberScoreViewModel : BaseScoreViewModel
    {
        public override int Rank => throw new NotImplementedException();

        public override string SongName => throw new NotImplementedException();

        public override bool FC => throw new NotImplementedException();

        public override int BadCuts => throw new NotImplementedException();

        public override int MissedNotes => throw new NotImplementedException();

        public override string Id => throw new NotImplementedException();

        public override string SubName => throw new NotImplementedException();

        public override string Author => throw new NotImplementedException();

        public override string Mapper => throw new NotImplementedException();

        public override string CoverImage => throw new NotImplementedException();

        public ScoreSaberScoreViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }


    }
}
