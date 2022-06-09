using CSM.Business.TwitchIntegration.TwitchConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Api;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace CSM.Business.TwitchIntegration
{
    public class TwitchChannelManager
    {
        private TwitchAPI twitchAPI;
        private List<TwitchChannel> connectedChannels;
        private ConnectionCredentials connectionCredentials;

        public event EventHandler<OnChatCommandReceivedArgs> OnChatCommandReceived;

        /// <summary>
        /// Adds a channel with the given id and name to the connected channels list.
        /// </summary>
        /// <param name="channelId">Unique identifier of the channel.</param>
        /// <param name="name">Name of the channel.</param>
        public void AddChannel(Guid channelId, string name)
        {
            if (!connectedChannels.Any(c => c.ChannelId == channelId))
            {
                var channel = new TwitchChannel(channelId, name);
                if (connectionCredentials == null) connectionCredentials = new ConnectionCredentials(TwitchConfigManager.Instance.Config.UserName, TwitchConfigManager.Instance.Config.AccessToken);
                channel.Initialize(connectionCredentials);
                channel.OnChatCommandReceived += Channel_OnChatCommandReceived;
            }
        }

        /// <summary>
        /// Removes the channel with the given id from the list of connected channels.
        /// </summary>
        /// <param name="channelId">Unique identifier of the channel to remove.</param>
        public void RemoveChannel(Guid channelId)
        {
            var connectedChannel = connectedChannels.SingleOrDefault(c => c.ChannelId == channelId);
            if (connectedChannel != null)
            {
                connectedChannel.OnChatCommandReceived -= Channel_OnChatCommandReceived;
                connectedChannel.CleanUp();
                connectedChannels.Remove(connectedChannel);
            }
        }

        #region Helper methods

        private void Setup()
        {
            twitchAPI = new TwitchAPI();
            twitchAPI.Settings.ClientId = "mf66rq31qva9bv7dit1jygdjs39loa";
            twitchAPI.Settings.AccessToken = TwitchConfigManager.Instance.Config.AccessToken;

            connectedChannels = new List<TwitchChannel>();
        }

        private void Channel_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            OnChatCommandReceived?.Invoke(sender, e);
        }

        #endregion

        #region Singleton

        private static TwitchChannelManager instance;

        public static TwitchChannelManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TwitchChannelManager();
                    instance.Setup();
                }
                return instance;
            }
        }

        #endregion
    }
}