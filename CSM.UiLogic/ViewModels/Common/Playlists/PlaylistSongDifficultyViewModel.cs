using CSM.DataAccess.Common;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Common.Playlists
{
    internal class PlaylistSongDifficultyViewModel : BaseViewModel
    {
        private bool isSelected;

        private readonly DataAccess.Playlists.Difficulty difficulty;

        public Characteristic Characteristic => difficulty.Characteristic;

        public Difficulty Difficulty => difficulty.Name;

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (value == isSelected)
                    return;
                isSelected = value;
                OnPropertyChanged();
                DifficultyChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler? DifficultyChanged;

        public PlaylistSongDifficultyViewModel(IServiceLocator serviceLocator, DataAccess.Playlists.Difficulty difficulty, bool isSelected = false) : base(serviceLocator)
        {
            this.difficulty = difficulty;
            IsSelected = isSelected;
        }
    }
}
