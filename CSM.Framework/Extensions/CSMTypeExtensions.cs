using CSM.Framework.Properties;

namespace CSM.Framework.Extensions
{
    public static class CSMTypeExtensions
    {
        public static string ToText(this WorkspaceType workspaceType)
        {
            switch (workspaceType)
            {
                case WorkspaceType.CustomLevels:
                    return Resources.Extension_CustomLevels;
                case WorkspaceType.Playlists:
                    return Resources.Extension_Playlists;
                case WorkspaceType.TwitchIntegration:
                    return Resources.Extension_Twitch;
                case WorkspaceType.Tools:
                    return Resources.Extension_Tools;
                default:
                    return string.Empty;
            }
        }

        public static string ToWorkspaceType(this string workspaceName)
        {
            switch (workspaceName)
            {
                case "CustomLevels":
                    return Resources.Extension_CustomLevels;
                case "Playlists":
                    return Resources.Extension_Playlists;
                case "TwitchIntegration":
                    return Resources.Extension_Twitch;
                case "Tools":
                    return Resources.Extension_Tools;
                default:
                    return string.Empty;
            }
        }
    }
}