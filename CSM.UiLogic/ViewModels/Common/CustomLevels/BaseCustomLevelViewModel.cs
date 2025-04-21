using CSM.DataAccess.BeatSaver;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.ViewModels.Common.MapDetails;
using System.Globalization;

namespace CSM.UiLogic.ViewModels.Common.CustomLevels
{
    internal abstract class BaseCustomLevelViewModel<TModel>(IServiceLocator serviceLocator, TModel model, string path, string bsrKey, DateTime lastwriteTime) : BaseViewModel(serviceLocator), ICustomLevelViewModel where TModel : class
    {
        private MapDetailViewModel? mapDetailViewModel;

        protected TModel Model { get; } = model;

        #region Properties

        public string Path { get; } = path;

        public string BsrKey { get; } = bsrKey;

        public int BsrKeyHex
        {
            get
            {
                int.TryParse(BsrKey, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int result);
                return result;
            }
        }

        public DateTime LastWriteTime { get; } = lastwriteTime;

        public abstract string Version { get; }

        public abstract string SongTitle { get; }

        public abstract string SongSubTitle { get; }

        public abstract string SongAuthor { get; }

        public abstract string LevelAuthor { get; }

        public abstract double Bpm { get; }

        public abstract bool HasEasyMap { get; }

        public abstract bool HasNormalMap { get; }

        public abstract bool HasHardMap { get; }

        public abstract bool HasExpertMap { get; }

        public abstract bool HasExpertPlusMap { get; }

        public MapDetailViewModel? MapDetailViewModel
        {
            get => mapDetailViewModel;
            set
            {
                if (mapDetailViewModel == value)
                    return;
                mapDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public void UpdateMapDetail(MapDetail mapDetail)
        {
            MapDetailViewModel = new MapDetailViewModel(mapDetail);
        }
    }
}
