using CSM.DataAccess.Entities.Types;
using CSM.UiLogic.Workspaces.Playlists;
using System.Windows;
using System.Windows.Controls;

namespace CSM.App.Workspaces.Common
{
    public class CharacteristicDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Degree360DataTemplate { get; set; }

        public DataTemplate Degree90DataTemplate { get; set; }

        public DataTemplate StandardDataTemplate { get; set; }

        public DataTemplate NoArrowsDataTemplate { get; set; }

        public DataTemplate OneSaberDataTemplate { get; set; }

        public DataTemplate LawlessDataTemplate { get; set; }

        public DataTemplate LightshowDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var characteristic = item as PlaylistSongDifficultyViewModel;
            if (characteristic == null) return base.SelectTemplate(item, container);
            if (characteristic.Characteristic == CharacteristicTypes.Degree360) return Degree360DataTemplate;
            if (characteristic.Characteristic == CharacteristicTypes.Degree90) return Degree90DataTemplate;
            if (characteristic.Characteristic == CharacteristicTypes.Standard) return StandardDataTemplate;
            if (characteristic.Characteristic == CharacteristicTypes.NoArrows) return NoArrowsDataTemplate;
            if (characteristic.Characteristic == CharacteristicTypes.OneSaber) return OneSaberDataTemplate;
            if (characteristic.Characteristic == CharacteristicTypes.Lawless) return LawlessDataTemplate;
            if (characteristic.Characteristic == CharacteristicTypes.Lightshow) return LightshowDataTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}