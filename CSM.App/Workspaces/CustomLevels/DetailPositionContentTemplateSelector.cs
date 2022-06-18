using CSM.Framework.Configuration.UserConfiguration;
using System.Windows;
using System.Windows.Controls;

namespace CSM.App.Workspaces.CustomLevels
{
    /// <summary>
    /// DataTemplateSelector to select song detail position to the right or bottom.
    /// </summary>
    public class DetailPositionContentTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Gets or sets the right position data template.
        /// </summary>
        public DataTemplate RightDataTemplate { get; set; }

        /// <summary>
        /// Gets or sets the bottom position data template.
        /// </summary>
        public DataTemplate BottomDataTemplate { get; set; }

        /// <summary>
        /// Selects the data template based on the user configuration.
        /// </summary>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (UserConfigManager.Instance.Config.CustomLevelsSongDetailPosition == Framework.SongDetailPosition.Right)
            {
                return RightDataTemplate;
            }
            else if (UserConfigManager.Instance.Config.CustomLevelsSongDetailPosition == Framework.SongDetailPosition.Bottom)
            {
                return BottomDataTemplate;
            }
            else return base.SelectTemplate(item, container);
        }
    }
}