using CSM.Business.TwitchIntegration.TwitchConfiguration;
using System;
using System.Linq;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace CSM.Business.TwitchIntegration
{
    public class TwitchChannelManager
    {
        private TwitchClient twitchClient;

        public bool IsConnected => twitchClient != null && twitchClient.IsConnected;

        public static event EventHandler<OnJoinedChannelArgs> OnJoinedChannel;

        public static event EventHandler<OnLeftChannelArgs> OnLeftChannel;

        public static event EventHandler<SongRequestEventArgs> OnBsrKeyReceived;

        /// <summary>
        /// Adds a channel with the given id and name to the connected channels list.
        /// </summary>
        /// <param name="channelId">Unique identifier of the channel.</param>
        /// <param name="channelName">Name of the channel.</param>
        public void JoinChannel(string channelName)
        {
            if (!twitchClient.JoinedChannels.Any(c => c.Channel == channelName))
            {
                twitchClient.JoinChannel(channelName);
            }
        }

        /// <summary>
        /// Removes the channel with the given id from the list of connected channels.
        /// </summary>
        /// <param name="channelName">The name of the channel to remove.</param>
        public void LeaveChannel(string channelName)
        {
            var channel = twitchClient.JoinedChannels.FirstOrDefault(c => c.Channel == channelName);
            if (channel != null)
            {
                twitchClient.LeaveChannel(channel);
            }
        }

        public bool CheckChannelIsJoined(string channelName)
        {
            return twitchClient.JoinedChannels.Any(c => c.Channel == channelName);
        }

        #region Helper methods

        private void Setup()
        {
            var connectionCredentials = new ConnectionCredentials("InnocentThief", TwitchConfigManager.Instance.Config.AccessToken);

            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            twitchClient = new TwitchClient(customClient);
            twitchClient.Initialize(connectionCredentials);
            twitchClient.OnLog += TwitchClient_OnLog;
            twitchClient.OnJoinedChannel += TwitchClient_OnJoinedChannel;
            twitchClient.OnLeftChannel += TwitchClient_OnLeftChannel;
            twitchClient.OnChatCommandReceived += TwitchClient_OnChatCommandReceived;
            twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            twitchClient.OnConnectionError += TwitchClient_OnConnectionError;
            twitchClient.OnConnected += TwitchClient_OnConnected;

            twitchClient.Connect();
        }

        private void TwitchClient_OnLog(object sender, OnLogArgs e)
        {
            Console.WriteLine($"{e.DateTime.ToString()}: {e.BotUsername} - {e.Data}");
        }

        private void TwitchClient_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            Console.WriteLine($"Custom Songs Manager connected to {e.Channel}");
            OnJoinedChannel?.Invoke(this, e);
            twitchClient.SendMessage(e.Channel, $"Custom Songs Manager connected to {e.Channel}");
        }

        private void TwitchClient_OnLeftChannel(object sender, OnLeftChannelArgs e)
        {
            Console.WriteLine($"Custom Songs Manager left {e.Channel}");
            OnLeftChannel?.Invoke(this, e);

        }

        private void TwitchClient_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            if (!e.Command.ArgumentsAsList.Any()) return;
            Console.WriteLine($"Received command {e.Command} with parameter {e.Command.ArgumentsAsList[0]}");
            var eventArgs = new SongRequestEventArgs
            {
                ChannelName = e.Command.ChatMessage.Channel,
                Key = e.Command.ArgumentsAsList[0]
            };
            OnBsrKeyReceived?.Invoke(sender, eventArgs);
        }

        private void TwitchClient_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (!e.ChatMessage.Message.StartsWith("!"))
            {
                Console.WriteLine($"Received message {e.ChatMessage.Message}");
            }
        }

        private void TwitchClient_OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine($"Custom Songs Manager connected to Twitch");
        }

        private void TwitchClient_OnConnectionError(object sender, OnConnectionErrorArgs e)
        {
            Console.WriteLine($"Error connection to Twitch: {e.Error}");
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