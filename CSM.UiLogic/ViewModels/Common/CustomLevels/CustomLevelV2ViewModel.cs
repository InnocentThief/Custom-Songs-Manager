using CSM.DataAccess.CustomLevels;
using CSM.Framework.ServiceLocation;

namespace CSM.UiLogic.ViewModels.Common.CustomLevels
{
    internal class CustomLevelV2ViewModel : BaseCustomLevelViewModel<InfoV2>
    {
        public override string Version => Model.Version;

        public override string SongTitle => Model.SongName;

        public override string SongSubTitle => Model.SongSubName;

        public override string SongAuthor => Model.SongAuthorName;

        public CustomLevelV2ViewModel(IServiceLocator serviceLocator, InfoV2 model, string path, string bsrKey, DateTime lastwriteTime) : base(serviceLocator, model, path, bsrKey, lastwriteTime)
        {
        }
    }
}
