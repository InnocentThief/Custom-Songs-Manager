using CSM.DataAccess.BeatSaver;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Controls.SongSources.SongSearch
{
    internal class LeaderboardItem(SearchParamLeaderboard? key, string name, bool isSelected = false) : BaseNotifiable
    {
        private bool isSelected = isSelected;

        public SearchParamLeaderboard? Key { get; } = key;

        public string Name { get; } = name;

        public bool None => Key == null;

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
}
