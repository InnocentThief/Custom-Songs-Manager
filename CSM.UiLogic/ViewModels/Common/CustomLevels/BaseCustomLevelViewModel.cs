using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Common.CustomLevels
{
    internal abstract class BaseCustomLevelViewModel<TModel>(IServiceLocator serviceLocator, TModel model, string path, string bsrKey, DateTime lastwriteTime) : BaseViewModel(serviceLocator), ICustomLevelViewModel where TModel : class
    {
        protected TModel Model { get; } = model;

        public string Path { get; } = path;

        public string BsrKey { get; } = bsrKey;

        public DateTime LastWriteTime { get; } = lastwriteTime;

        public abstract string Version { get; }

        public abstract string SongTitle { get; }

        public abstract string SongSubTitle { get; }

        public abstract string SongAuthor { get; }
    }
}
