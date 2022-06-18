using System.Collections.Generic;

namespace CSM.Business.TwitchIntegration.TwitchConfiguration
{
    /// <summary>
    /// Represents the Twitch configuration.
    /// </summary>
    public class TwitchConfig
    {
        /// <summary>
        /// Gets or sets the Twitch user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the Twitch access token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the Twitch refresh token.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the Twitch login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the Twitch user id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Contains the configured channels.
        /// </summary>
        public List<TwitchChannel> Channels { get; set; }
    }
}