using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Controls.SongSources
{
    internal class BeatSaberFavouritesSourceViewModel : BaseViewModel, ISongSourceViewModel
    {
        public BeatSaberFavouritesSourceViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public async Task LoadAsync()
        {

        }
    }
}
