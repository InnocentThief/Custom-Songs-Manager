using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Common.Playlists
{
    internal abstract class BasePlaylistViewModel(IServiceLocator serviceLocator, string name, string path) : BaseViewModel(serviceLocator)
    {
        private string name = name;

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
    }
}
