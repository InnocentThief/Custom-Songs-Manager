using System.Collections.ObjectModel;
using CSM.DataAccess.UserConfiguration;
using CSM.Framework.Extensions;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using CSM.UiLogic.Helper;

namespace CSM.UiLogic.ViewModels.Controls.Settings
{
    internal class LeaderboardsSettingsViewModel : BaseViewModel
    {
        private readonly UserConfig userConfig;

        public string ScoreSaberUserId
        {
            get => userConfig.LeaderboardsConfig.ScoreSaberUserId;
            set
            {
                if (value == userConfig.LeaderboardsConfig.ScoreSaberUserId)
                    return;
                userConfig.LeaderboardsConfig.ScoreSaberUserId = value;
                OnPropertyChanged();
            }
        }
        public string BeatLeaderUserId
        {
            get => userConfig.LeaderboardsConfig.BeatLeaderUserId;
            set
            {
                if (value == userConfig.LeaderboardsConfig.BeatLeaderUserId)
                    return;
                userConfig.LeaderboardsConfig.BeatLeaderUserId = value;
                OnPropertyChanged();
            }
        }
        public bool UseScoreSaberLeaderboard
        {
            get => userConfig.LeaderboardsConfig.UseScoreSaberLeaderboard;
            set
            {
                if (value == userConfig.LeaderboardsConfig.UseScoreSaberLeaderboard)
                    return;
                userConfig.LeaderboardsConfig.UseScoreSaberLeaderboard = value;
                OnPropertyChanged();
            }
        }
        public bool UseBeatLeaderLeaderboard
        {
            get => userConfig.LeaderboardsConfig.UseBeatLeaderLeaderboard;
            set
            {
                if (value == userConfig.LeaderboardsConfig.UseBeatLeaderLeaderboard)
                    return;
                userConfig.LeaderboardsConfig.UseBeatLeaderLeaderboard = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<EnumWrapper<LeaderboardType>> Leaderboards { get; } = [];

        public EnumWrapper<LeaderboardType>? SelectedLeaderboard
        {
            get => Leaderboards.FirstOrDefault(x => x.Value == userConfig.LeaderboardsConfig.DefaultLeaderboard);
            set
            {
                if (value == null)
                    return;
                if (value.Value == userConfig.LeaderboardsConfig.DefaultLeaderboard)
                    return;
                userConfig.LeaderboardsConfig.DefaultLeaderboard = value.Value;
                OnPropertyChanged();
            }
        }

        public LeaderboardsSettingsViewModel(IServiceLocator serviceLocator, UserConfig userConfig) : base(serviceLocator)
        {
            this.userConfig = userConfig;

            Leaderboards.AddRange(EnumWrapper<LeaderboardType>.GetValues(serviceLocator, LeaderboardType.None));
        }
    }
}

