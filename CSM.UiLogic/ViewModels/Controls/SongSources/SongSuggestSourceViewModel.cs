using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Controls.SongSources
{
    internal class SongSuggestSourceViewModel : BaseViewModel, ISongSourceViewModel
    {
        public SongSuggestSourceViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }
    }
}
