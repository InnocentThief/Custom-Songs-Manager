using CSM.Framework;
using CSM.UiLogic.Workspaces;
using System.Windows;
using System.Windows.Controls;

namespace CSM.App.Workspaces
{
    internal class WorkspaceContentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CustomLevels { get; set; }

        public DataTemplate Playlists { get; set; }

        public DataTemplate TwitchIntegration { get; set; }

        public DataTemplate Tools { get; set; }

        public DataTemplate SongSearch { get; set; }

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
                case WorkspaceType.SongSearch:
                    return SongSearch;
                default:
                    return base.SelectTemplate(item, container);
            }
        }
    }
}