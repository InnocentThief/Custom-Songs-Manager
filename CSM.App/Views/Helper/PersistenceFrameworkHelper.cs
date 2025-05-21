using Telerik.Windows.Controls;
using Telerik.Windows.Persistence;

namespace CSM.App.Views.Helper
{
    internal static class PersistenceFrameworkHelper
    {

        public static PersistenceManager GetPersistenceManager()
        {
            return new PersistenceManager()
                .AllowDataAssembly()
                .AllowCoreControls()
                .AllowGridViewControls()
                .AllowTypes(
                    typeof(ColumnProxy),
                    typeof(SortDescriptorProxy),
                    typeof(GroupDescriptorProxy),
                    typeof(FilterDescriptorProxy),
                    typeof(FilterSetting),
                    typeof(List<ColumnProxy>),
                    typeof(List<SortDescriptorProxy>),
                    typeof(List<GroupDescriptorProxy>),
                    typeof(List<FilterDescriptorProxy>),
                    typeof(List<FilterSetting>),
                    typeof(List<object>)
                );

        }


    }
}
