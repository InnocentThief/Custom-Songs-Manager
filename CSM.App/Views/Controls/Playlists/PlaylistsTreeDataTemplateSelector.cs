using CSM.UiLogic.ViewModels.Common.Playlists;
using System.Windows;
using System.Windows.Controls;

namespace CSM.App.Views.Controls.Playlists
{
    internal class PlaylistsTreeDataTemplateSelector : DataTemplateSelector
    {
        public HierarchicalDataTemplate FolderTemplate { get; set; } = null!;
        public DataTemplate PlaylistTemplate { get; set; } = null!;
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is PlaylistFolderViewModel)
                return FolderTemplate;
            else if (item is PlaylistViewModel)
                return PlaylistTemplate;
            return base.SelectTemplate(item, container);
        }
    }
}
