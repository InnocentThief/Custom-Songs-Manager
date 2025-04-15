using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Controls.SongSources
{
    internal class SongSearchSourceViewModel : BaseViewModel, ISongSourceViewModel
    {
        public SongSearchSourceViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }
    }
}
