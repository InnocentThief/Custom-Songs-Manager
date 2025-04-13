using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.ViewModels.Controls.SongSources
{
    internal class SongSearchSourceViewModel : BaseViewModel, ISongSourceViewModel
    {
        public SongSearchSourceViewModel(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }
    }
}
