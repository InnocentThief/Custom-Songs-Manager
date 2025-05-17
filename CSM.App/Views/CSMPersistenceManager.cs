using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;
using Telerik.Windows.Persistence.Services;
using Telerik.Windows.Persistence;
using Telerik.Windows.Controls;

namespace CSM.App.Views
{
    internal class CSMPersistenceManager: PersistenceManager
    {
        //public static PersistenceManager GetInstance()
        //{
        //    return new PersistenceManager()
        //        .AllowDataAssembly()
        //        .AllowCoreControls()
        //        .AllowGridViewControls()
        //        .AllowTypes(
        //            typeof(ColumnProxy),
        //            typeof(SortDescriptorProxy),
        //            typeof(GroupDescriptorProxy),
        //            typeof(FilterDescriptorProxy),
        //            typeof(FilterSetting),
        //            typeof(List<ColumnProxy>),
        //            typeof(List<SortDescriptorProxy>),
        //            typeof(List<GroupDescriptorProxy>),
        //            typeof(List<FilterDescriptorProxy>),
        //            typeof(List<FilterSetting>),
        //            typeof(List<object>)
        //        );
        //}
    }
}
