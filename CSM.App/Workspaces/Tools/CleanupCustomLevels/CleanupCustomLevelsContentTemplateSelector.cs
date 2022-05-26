using CSM.UiLogic.Workspaces.Tools.CleanupCustomLevels;
using System.Windows;
using System.Windows.Controls;

namespace CSM.App.Workspaces.Tools.CleanupCustomLevels
{
    public class CleanupCustomLevelsContentTemplateSelector: DataTemplateSelector
    {
        public DataTemplate StartTemplate { get; set; }

        public DataTemplate DirectoryNamesTemplate { get; set; }

        public DataTemplate DuplicatesTemplate { get; set; }

        public DataTemplate VersionsTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is StepStartViewModel)
            {
                return StartTemplate;
            }
            else if (item is StepDirectoryNamesViewModel)
            {
                return DirectoryNamesTemplate;
            }
            else if (item is StepDuplicatesViewModel)
            {
                return DuplicatesTemplate;
            }
            else if (item is StepVersionsViewModel)
            {
                return VersionsTemplate;
            }
            else
            {
                return base.SelectTemplate(item, container);
            }
        }
    }
}