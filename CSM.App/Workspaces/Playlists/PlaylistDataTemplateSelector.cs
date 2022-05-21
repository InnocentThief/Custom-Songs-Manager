using CSM.UiLogic.Workspaces.Playlists;
using System.Windows;
using System.Windows.Controls;

namespace CSM.App.Workspaces.Playlists
{
    public class PlaylistDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PlaylistTemplate { get; set; }

        public DataTemplate NoDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is PlaylistViewModel)
            {
                return PlaylistTemplate;
            }
            else
            {
                return NoDataTemplate;
            }
        }
    }
}