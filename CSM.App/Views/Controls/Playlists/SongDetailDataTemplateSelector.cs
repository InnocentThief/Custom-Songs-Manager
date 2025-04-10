using CSM.UiLogic.ViewModels.Common.Playlists;
using System.Windows;
using System.Windows.Controls;

namespace CSM.App.Views.Controls.Playlists
{
   internal class SongDetailDataTemplateSelector: DataTemplateSelector
    {
        public DataTemplate SongDataTemplate { get; set; } = null!;
        public DataTemplate NoSongDataTemplate { get; set; } = null!;
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if(item is PlaylistSongViewModel)
                return SongDataTemplate;
            else 
                return NoSongDataTemplate;
        }
    }
}
