using CSM.UiLogic.ViewModels.Common.Playlists;
using System.Windows;
using System.Windows.Controls;

namespace CSM.App.Views.Controls.Playlists
{
   internal class PlaylistDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate NoPlaylistDataTemplate { get; set; } = null!;
        public DataTemplate FolderDataTemplate { get; set; } = null!;
        public DataTemplate PlaylistDataTemplate { get; set; } = null!;
        public override DataTemplate SelectTemplate(object? item, DependencyObject container)
        {
            if (item is null) return NoPlaylistDataTemplate;
            if (item is PlaylistViewModel) return PlaylistDataTemplate;
            if (item is PlaylistFolderViewModel) return FolderDataTemplate;
            return base.SelectTemplate(item, container);
        }
    }
}
