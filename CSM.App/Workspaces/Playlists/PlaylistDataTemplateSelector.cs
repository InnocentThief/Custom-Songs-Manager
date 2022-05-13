using CSM.UiLogic.Workspaces.Playlists;
using System.Windows;
using System.Windows.Controls;

namespace CSM.App.Workspaces.Playlists
{
    public class PlaylistDataTemplateSelector : DataTemplateSelector
    {
        public HierarchicalDataTemplate FolderTemplate { get; set; }

        public HierarchicalDataTemplate PlaylistTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is PlaylistFolderViewModel)
            {
                return FolderTemplate;
            }
            else if (item is PlaylistViewModel)
            {
                return PlaylistTemplate;
            }
            else
            {
                return base.SelectTemplate(item, container);
            }

        }
    }
}