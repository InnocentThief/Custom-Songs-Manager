using CSM.DataAccess.UserConfiguration;
using CSM.Framework.Extensions;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Helper;
using System.Collections.ObjectModel;

namespace CSM.UiLogic.ViewModels.Controls.Settings
{
    internal class SongSuggestSettingsViewModel : BaseViewModel
    {
        private readonly UserConfig userConfig;

        #region Properties

        public string ScoreSaberUserId
        {
            get => userConfig.SongSuggestConfig.ScoreSaberUserId;
            set
            {
                if (value == userConfig.SongSuggestConfig.ScoreSaberUserId)
                    return;
                userConfig.SongSuggestConfig.ScoreSaberUserId = value;
                OnPropertyChanged();
            }
        }
        public string BeatLeaderUserId
        {
            get => userConfig.SongSuggestConfig.BeatLeaderUserId;
            set
            {
                if (value == userConfig.SongSuggestConfig.BeatLeaderUserId)
                    return;
                userConfig.SongSuggestConfig.BeatLeaderUserId = value;
                OnPropertyChanged();
            }
        }
        public bool UseScoreSaberLeaderboard
        {
            get => userConfig.SongSuggestConfig.UseScoreSaberLeaderboard;
            set
            {
                if (value == userConfig.SongSuggestConfig.UseScoreSaberLeaderboard)
                    return;
                userConfig.SongSuggestConfig.UseScoreSaberLeaderboard = value;
                OnPropertyChanged();
            }
        }
        public bool UseBeatLeaderLeaderboard
        {
            get => userConfig.SongSuggestConfig.UseBeatLeaderLeaderboard;
            set
            {
                if (value == userConfig.SongSuggestConfig.UseBeatLeaderLeaderboard)
                    return;
                userConfig.SongSuggestConfig.UseBeatLeaderLeaderboard = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<EnumWrapper<LeaderboardType>> Leaderboards { get; } = [];

        public EnumWrapper<LeaderboardType>? SelectedLeaderboard
        {
            get => Leaderboards.FirstOrDefault(x => x.Value == userConfig.SongSuggestConfig.DefaultLeaderboard);
            set
            {
                if (value == null)
                    return;
                if (value.Value == userConfig.SongSuggestConfig.DefaultLeaderboard)
                    return;
                userConfig.SongSuggestConfig.DefaultLeaderboard = value.Value;
                OnPropertyChanged();
            }
        }

        #endregion

        public SongSuggestSettingsViewModel(IServiceLocator serviceLocator, UserConfig userConfig) : base(serviceLocator)
        {
            this.userConfig = userConfig;

            Leaderboards.AddRange(EnumWrapper<LeaderboardType>.GetValues(serviceLocator));
        }
    }
}
