using CSM.DataAccess.CustomLevels;
using CSM.Framework.ServiceLocation;

namespace CSM.UiLogic.ViewModels.Common.CustomLevels
{
    internal sealed class CustomLevelV4ViewModel : BaseCustomLevelViewModel<InfoV4>
    {
        public override string Version => Model.Version;

        public override string SongTitle => Model.Song.Title;

        public override string SongSubTitle => Model.Song.SubTitle;

        public override string SongAuthor => Model.Song.Author;

        public CustomLevelV4ViewModel(IServiceLocator serviceLocator, InfoV4 model, string path, string bsrKey, DateTime lastwriteTime) : base(serviceLocator, model, path, bsrKey, lastwriteTime)
        {
        }
    }
}
