using CSM.DataAccess.BeatSaver;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Controls.SongSources.SongSearch
{
    internal class StyleItem(Tag key, string name, bool isSelected = false) : BaseNotifiable
    {
        private bool isSelected = isSelected;

        public Tag Key { get; } = key;

        public string Name { get; } = name;

        public bool None => Key == Tag.None;

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
