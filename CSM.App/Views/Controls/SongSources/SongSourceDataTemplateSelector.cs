using System.Windows;
using System.Windows.Controls;
using CSM.UiLogic.ViewModels.Controls.CustomLevels;
using CSM.UiLogic.ViewModels.Controls.SongSources;

namespace CSM.App.Views.Controls.SongSources
{
    internal class SongSourceDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CustomLevelsDataTemplate { get; set; } = null!;
        public DataTemplate PlaylistsDataTemplate { get; set; } = null!;
        public DataTemplate FavouritesDataTemplate { get; set; } = null!;
        public DataTemplate SearchDataTemplate { get; set; } = null!;
        public DataTemplate SongSuggestDataTemplate { get; set; } = null!;
        public DataTemplate TwitchDataTemplate { get; set; } = null!;

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return base.SelectTemplate(item, container);
            if (item is CustomLevelsControlViewModel)
                return CustomLevelsDataTemplate;
            if (item is PlaylistsSourceViewModel)
                return PlaylistsDataTemplate;
            if (item is BeatSaberFavouritesSourceViewModel)
                return FavouritesDataTemplate;
            if (item is SongSearchSourceViewModel)
                return SearchDataTemplate;
            if (item is SongSuggestSourceViewModel)
                return SongSuggestDataTemplate;
            if (item is TwitchSourceViewModel)
                return TwitchDataTemplate;
            return base.SelectTemplate(item, container);
        }
    }
}
