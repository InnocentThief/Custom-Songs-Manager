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
            var songSource = (SongSource)item;
            return songSource switch
            {
                SongSource.CustomLevels => CustomLevelsDataTemplate,
                SongSource.Playlists => PlaylistsDataTemplate,
                SongSource.Favourites => FavouritesDataTemplate,
                SongSource.Search => SearchDataTemplate,
                SongSource.SongSuggest => SongSuggestDataTemplate,
                _ => base.SelectTemplate(item, container),
            };
        }
    }
}
