using CSM.Business.Core.SongSelection;
using CSM.Business.Interfaces.SongCopy;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Common.Playlists
{
    internal abstract class BasePlaylistViewModel(
        IServiceLocator serviceLocator,
        string name,
        string path)
        : BaseViewModel(serviceLocator), IBasePlaylistViewModel
    {
        #region Private fields

        private string name = name;
        private bool containsLeftSong, containsRightSong;

        #endregion

        #region Properties

        public string Name
        {
            get => name;
            set
            {
                if (value == name) return;
                name = value;
                OnPropertyChanged();
            }
        }

        public string Path { get; } = path;

        public bool ContainsLeftSong
        {
            get => containsLeftSong;
            set
            {
                if (value == containsLeftSong) return;
                containsLeftSong = value;
                OnPropertyChanged();
            }
        }

        public bool ContainsRightSong
        {
            get => containsRightSong;
            set
            {
                if (value == containsRightSong) return;
                containsRightSong = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public abstract bool CheckContainsSong(string? hash, SongSelectionType songSelectionType);

        public abstract void CleanUpReferences();
    }
}
