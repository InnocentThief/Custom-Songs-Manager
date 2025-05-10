using System.Windows;
using System.Windows.Controls;
using CSM.DataAccess.UserConfiguration;
using CSM.UiLogic.ViewModels.Controls.CustomLevels;

namespace CSM.App.Views.Workspaces.CustomLevels
{
    internal class SongDetailPositionContentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SongDetailRightDataTemplate { get; set; } = null!;
        public DataTemplate SongDetailBottomDataTemplate { get; set; } = null!;

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is CustomLevelsControlViewModel customLevelsControlViewModel)
            {
                switch (customLevelsControlViewModel.SongDetailPosition)
                {
                    case SongDetailPosition.Right:
                        return SongDetailRightDataTemplate;
                    case SongDetailPosition.Bottom:
                        return SongDetailBottomDataTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
