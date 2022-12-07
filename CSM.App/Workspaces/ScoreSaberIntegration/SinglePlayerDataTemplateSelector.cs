using System.Windows;
using System.Windows.Controls;

namespace CSM.App.Workspaces.ScoreSaberIntegration
{
    public class SinglePlayerDataTemplateSelector: DataTemplateSelector
    {
        public DataTemplate NoSinglePlayerDataTemplate { get; set; }

        public DataTemplate SinglePlayerDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return NoSinglePlayerDataTemplate;
            return SinglePlayerDataTemplate;
        }
    }
}