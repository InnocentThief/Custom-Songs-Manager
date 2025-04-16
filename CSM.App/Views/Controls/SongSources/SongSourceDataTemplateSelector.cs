using CSM.UiLogic.ViewModels.Controls.CustomLevels;
using CSM.UiLogic.ViewModels.Controls.SongSources;
using System.Windows;
using System.Windows.Controls;

namespace CSM.App.Views.Controls.SongSources
{
    internal class SongSourceDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CustomLevelsDataTemplate { get; set; } = null!;
        public DataTemplate PlaylistsDataTemplate { get; set; } = null!;
        public DataTemplate FavouritesDataTemplate { get; set; } = null!;
        public DataTemplate SearchDataTemplate { get; set; } = null!;
        public DataTemplate SongSuggestDataTemplate { get; set; } = null!;
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return base.SelectTemplate(item, container);
            if (item is CustomLevelsControlViewModel customLevelsControlViewModel)
                return CustomLevelsDataTemplate;
            if (item is PlaylistsSourceViewModel playlistsSourceViewModel)
                return PlaylistsDataTemplate;
            if (item is BeatSaberFavouritesSourceViewModel beatSaberFavouritesSourceViewModel)
                return FavouritesDataTemplate;
            if (item is SongSearchSourceViewModel songSearchSourceViewModel)
                return SearchDataTemplate;
            if (item is SongSuggestSourceViewModel songSuggestSourceViewModel)
                return SongSuggestDataTemplate;
            return base.SelectTemplate(item, container);
        }
    }
}
