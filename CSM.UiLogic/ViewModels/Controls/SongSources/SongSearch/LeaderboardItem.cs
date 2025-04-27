using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Controls.SongSources.SongSearch
{
    internal class LeaderboardItem(LeaderboardItemType key, string name, bool isSelected = false) : BaseNotifiable
    {
        private bool isSelected = isSelected;

        public LeaderboardItemType Key { get; } = key;

        public string Name { get; } = name;

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (value == isSelected)
                    return;
                isSelected = value;
                OnPropertyChanged();
            }
        }
    }

    internal enum LeaderboardItemType
    {
        All = 0,
        Ranked = 1,
        BeatLeader = 2,
        ScoreSaber = 3
    }
}
