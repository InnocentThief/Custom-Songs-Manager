using CSM.Business.TwitchIntegration.TwitchConfiguration;
using CSM.Framework.Logging;
using System;
using System.Linq;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace CSM.Business.TwitchIntegration
{
    /// <summary>
    /// Handles Twitch channels.
    /// </summary>
    public class TwitchChannelManager
    {
        private TwitchClient twitchClient;
        const string beatsaverUri = "https://beatsaver.com/maps/";

        /// <summary>
        /// Gets whether the Twitch client is connected.
        /// </summary>
        public bool IsConnected => twitchClient != null && twitchClient.IsConnected;

        /// <summary>
        /// Occurs on Twitch channel joined.
        /// </summary>
        public static event EventHandler<OnJoinedChannelArgs> OnJoinedChannel;

        /// <summary>
        /// Occurs on Twitch channel left.
        /// </summary>
        public static event EventHandler<OnLeftChannelArgs> OnLeftChannel;

        /// <summary>
        /// Occurs on BSR Key received.
        /// </summary>
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

        /// <summary>
        /// Checks whether the channel with the given name is currently joined.
        /// </summary>
        /// <param name="channelName">The name of the channel to check.</param>
        /// <returns>True if the channel is joined; otherwise false.</returns>
        public bool CheckChannelIsJoined(string channelName)
        {
            return twitchClient.JoinedChannels.Any(c => c.Channel == channelName);
        }

        #region Helper methods

        private void Setup()
        {
            if (string.IsNullOrWhiteSpace(TwitchConfigManager.Instance.Config.UserName) || string.IsNullOrWhiteSpace(TwitchConfigManager.Instance.Config.AccessToken)) return;

            var connectionCredentials = new ConnectionCredentials(TwitchConfigManager.Instance.Config.UserName, TwitchConfigManager.Instance.Config.AccessToken);

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
            //twitchClient.SendMessage(e.Channel, $"Custom Songs Manager connected to {e.Channel}");
        }

        private void TwitchClient_OnLeftChannel(object sender, OnLeftChannelArgs e)
        {
            Console.WriteLine($"Custom Songs Manager left {e.Channel}");
            LoggerProvider.Logger.Info<TwitchChannelManager>($"Custom Songs Manager left {e.Channel}");
            OnLeftChannel?.Invoke(this, e);

        }

        private void TwitchClient_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            if (e.Command.ChatMessage.Message.StartsWith("! ")) return;
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
            if (e.ChatMessage.Message.Contains(beatsaverUri)) // probably a response to a !link request
            {
                var startIndex = e.ChatMessage.Message.IndexOf(beatsaverUri);
                var bsr = e.ChatMessage.Message.Substring(startIndex + beatsaverUri.Length, e.ChatMessage.Message.Length - startIndex - beatsaverUri.Length);
                Console.WriteLine($"Received message {e.ChatMessage.Message}");
                Console.WriteLine($"Extracted BSR key {bsr}");
                var eventArgs = new SongRequestEventArgs
                {
                    ChannelName = e.ChatMessage.Channel,
                    Key = bsr
                };
                OnBsrKeyReceived?.Invoke(sender, eventArgs);
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