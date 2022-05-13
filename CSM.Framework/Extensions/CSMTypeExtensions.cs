namespace CSM.Framework.Extensions
{
    public static class CSMTypeExtensions
    {
        public static string ToText(this WorkspaceType workspaceType)
        {
            switch (workspaceType)
            {
                case WorkspaceType.CustomLevels:
                    return "Custom Levels";
                case WorkspaceType.Playlists:
                    return "Playlists";
                case WorkspaceType.TwitchIntegration:
                    return "Twitch Integration";
                case WorkspaceType.Tools:
                    return "Tools";
                default:
                    return string.Empty;
            }
        }

        public static string ToWorkspaceType(this string workspaceName)
        {
            switch (workspaceName)
            {
                case "CustomLevels":
                    return "Custom Levels";
                case "Playlists":
                    return "Playlists";
                case "TwitchIntegration":
                    return "Twitch Integration";
                case "Tools":
                    return "Tools";
                default:
                    return string.Empty;
            }
        }
    }
}