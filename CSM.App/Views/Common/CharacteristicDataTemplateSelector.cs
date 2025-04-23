using CSM.DataAccess.Common;
using System.Windows;
using System.Windows.Controls;

namespace CSM.App.Views.Common
{
    internal class CharacteristicDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Degree360DataTemplate { get; set; } = null!;
        public DataTemplate Degree90DataTemplate { get; set; } = null!;
        public DataTemplate StandardDataTemplate { get; set; } = null!;
        public DataTemplate NoArrowsDataTemplate { get; set; } = null!;
        public DataTemplate OneSaberDataTemplate { get; set; } = null!;
        public DataTemplate LawlessDataTemplate { get; set; } = null!;
        public DataTemplate LightshowDataTemplate { get; set; } = null!;
        public DataTemplate LegacyDataTemplate { get; set; } = null!;

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
                return base.SelectTemplate(item, container);

            var characteristic = (Characteristic)item;

            if (characteristic == Characteristic.Degree360) return Degree360DataTemplate;
            if (characteristic == Characteristic.Degree90) return Degree90DataTemplate;
            if (characteristic == Characteristic.Standard) return StandardDataTemplate;
            if (characteristic == Characteristic.NoArrows) return NoArrowsDataTemplate;
            if (characteristic == Characteristic.OneSaber) return OneSaberDataTemplate;
            if (characteristic == Characteristic.Lawless) return LawlessDataTemplate;
            if (characteristic == Characteristic.Lightshow) return LightshowDataTemplate;
            if (characteristic == Characteristic.Legacy) return LegacyDataTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}
