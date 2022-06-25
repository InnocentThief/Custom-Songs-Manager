using CSM.Framework;
using CSM.UiLogic.Workspaces;
using System.Windows;
using System.Windows.Controls;

namespace CSM.App.Workspaces
{
    /// <summary>
    /// DataTemplateSelector for workspaces.
    /// </summary>
    internal class WorkspaceContentTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Gets or sets the custom levels workspace data template.
        /// </summary>
        public DataTemplate CustomLevels { get; set; }

        /// <summary>
        /// Gets or sets the playlists workspace data template.
        /// </summary>
        public DataTemplate Playlists { get; set; }

        /// <summary>
        /// Gets or sets the Twitch integration workspace data template.
        /// </summary>
        public DataTemplate TwitchIntegration { get; set; }

        /// <summary>
        /// Gets or sets the tools workspace data template.
        /// </summary>
        public DataTemplate Tools { get; set; }

        /// <summary>
        /// Selects the data template base on the selected workspace.
        /// </summary>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!(item is BaseWorkspaceViewModel workspaceViewModel)) return base.SelectTemplate(item, container);

            switch (workspaceViewModel.WorkspaceType)
            {
                case WorkspaceType.CustomLevels:
                    return CustomLevels;
                case WorkspaceType.Playlists:
                    return Playlists;
                case WorkspaceType.TwitchIntegration:
                    return TwitchIntegration;
                case WorkspaceType.Tools:
                    return Tools;
                default:
                    return base.SelectTemplate(item, container);
            }
        }
    }
}