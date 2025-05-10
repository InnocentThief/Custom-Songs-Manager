using System.Collections.ObjectModel;
using CSM.DataAccess.UserConfiguration;
using CSM.Framework.Extensions;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Helper;

namespace CSM.UiLogic.ViewModels.Controls.Settings
{
    internal class CustomLevelsSettingsViewModel : BaseViewModel
    {
        private readonly UserConfig userConfig;

        #region Properties

        public bool Available
        {
            get => userConfig.CustomLevelsConfig.Available;
            set
            {
                if (value == userConfig.CustomLevelsConfig.Available)
                    return;
                userConfig.CustomLevelsConfig.Available = value;
                OnPropertyChanged();
            }
        }

        public string CustomLevelsPath
        {
            get => userConfig.CustomLevelsConfig.CustomLevelPath.Path;
            set
            {
                if (value == userConfig.CustomLevelsConfig.CustomLevelPath.Path)
                    return;
                userConfig.CustomLevelsConfig.CustomLevelPath.Path = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<EnumWrapper<SongDetailPosition>> SongDetailPositions { get; } = [];

        public EnumWrapper<SongDetailPosition> SelectedSongDetailPosition
        {
            get => SongDetailPositions.FirstOrDefault(x => x.Value == userConfig.CustomLevelsConfig.SongDetailPosition) ?? SongDetailPositions[0];
            set
            {
                if (value == SelectedSongDetailPosition)
                    return;
                userConfig.CustomLevelsConfig.SongDetailPosition = value.Value;
                OnPropertyChanged();
            }
        }

        #endregion

        public CustomLevelsSettingsViewModel(IServiceLocator serviceLocator, UserConfig userConfig) : base(serviceLocator)
        {
            this.userConfig = userConfig;

            SongDetailPositions.AddRange(EnumWrapper<SongDetailPosition>.GetValues(serviceLocator));
        }
    }
}
