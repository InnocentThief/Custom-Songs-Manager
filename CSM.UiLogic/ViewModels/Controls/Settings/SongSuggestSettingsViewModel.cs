using CSM.DataAccess.UserConfiguration;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Controls.Settings
{
    internal class SongSuggestSettingsViewModel(IServiceLocator serviceLocator, UserConfig userConfig) : BaseViewModel(serviceLocator)
    {
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
    }
}
