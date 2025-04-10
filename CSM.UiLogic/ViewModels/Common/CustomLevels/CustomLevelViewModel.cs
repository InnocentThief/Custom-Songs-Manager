using CSM.DataAccess.CustomLevels;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.ViewModels.Common.CustomLevels
{
    internal class CustomLevelViewModel : BaseViewModel
    {
        private readonly string path;
        private readonly CustomLevel customLevel;
        private readonly string bsrKey;
        private readonly DateTime lastWriteTime;

        public CustomLevelViewModel(IServiceLocator serviceLocator, string path, CustomLevel customLevel, string bsrKey, DateTime lastWriteTime) : base(serviceLocator)
        {
            this.path = path;
            this.customLevel = customLevel; 
            this.bsrKey = bsrKey;
            this.lastWriteTime = lastWriteTime;
        }
    }
}
