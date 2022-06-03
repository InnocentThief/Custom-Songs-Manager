using CSM.Framework.Properties;

namespace CSM.Framework.Extensions
{
    /// <summary>
    /// Extensions for workspaces.
    /// </summary>
    public static class CSMTypeExtensions
    {
        /// <summary>
        /// Gets the name for the given workspace type.
        /// </summary>
        /// <param name="workspaceType">Workspace type to get the name for.</param>
        /// <returns>A string with the name for the given workspace type.</returns>
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

        /// <summary>
        /// Gets the name of a workspace for the given string.
        /// </summary>
        /// <param name="workspaceName">The string to get the workspace name for.</param>
        /// <returns>A string with the name for the given workspacename.</returns>
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