using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Controls.SongSources.SongSearch
{
    internal class EnvironmentItem(DataAccess.BeatSaver.Environment key, string name, bool isSelected = false) : BaseNotifiable
    {
        private bool isSelected = isSelected;

        public DataAccess.BeatSaver.Environment Key { get; } = key;

        public string Name { get; } = name;

        public bool None => Key == DataAccess.BeatSaver.Environment.None;

        public bool All => Key == DataAccess.BeatSaver.Environment.All;

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
