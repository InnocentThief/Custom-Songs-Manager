using CSM.Business.Core.Twitch;
using CSM.Business.Interfaces;
using CSM.DataAccess.UserConfiguration;
using Microsoft.Extensions.Logging;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace CSM.Business.Core
{
    internal class TwitchChannelService(IUserConfigDomain userConfigDomain, ILogger<TwitchChannelService> logger) : ITwitchChannelService
    {
        private TwitchClient? twitchClient;
        const string beatsaverUri = "https://beatsaver.com/maps/";

        public event EventHandler<OnJoinedChannelArgs>? OnJoinedChannel;

        public event EventHandler<OnLeftChannelArgs>? OnLeftChannel;

        public event EventHandler<SongRequestEventArgs>? OnBsrKeyReceived;

        public event EventHandler<OnConnectedArgs>? OnConnected;

        public void AddChannel(string channelName)
        {
            if (userConfigDomain.Config?.TwitchConfig.Channels.Find(c => c.Name == channelName) != null)
                return;
            userConfigDomain.Config!.TwitchConfig.Channels.Add(new TwitchChannel { Name = channelName });
            userConfigDomain.SaveUserConfig();
        }

        public void AddSong(string key, string channelName, DateTime receivedAt)
        {
            if (userConfigDomain.Config?.TwitchConfig.Songs.Find(s => s.Key == key) != null)
                return;
            userConfigDomain.Config!.TwitchConfig.Songs.Add(new TwitchSong { Key = key, ChannelName = channelName, ReceivedAt = receivedAt });
            userConfigDomain.SaveUserConfig();
        }

        public bool CheckChannelIsJoined(string channelName)
        {
            return twitchClient != null && twitchClient.JoinedChannels.Any(c => c.Channel == channelName);
        }

        public bool Initialize()
        {
            try
            {
                if (twitchClient != null)
                    return true;

                var username = userConfigDomain.Config?.TwitchConfig.Username;
                var accessToken = userConfigDomain.Config?.TwitchConfig.AccessToken;
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(accessToken))
                    return false;

                var credentials = new ConnectionCredentials(username, accessToken);
                var clientOptions = new ClientOptions
                {
                    MessagesAllowedInPeriod = 750,
                    ThrottlingPeriod = TimeSpan.FromSeconds(30),
                };
                WebSocketClient webSocketClient = new(clientOptions);
                twitchClient = new TwitchClient(webSocketClient);
                twitchClient.Initialize(credentials);
                twitchClient.OnConnected += TwitchClient_OnConnected;
                twitchClient.OnConnectionError += TwitchClient_OnConnectionError;
                twitchClient.OnJoinedChannel += TwitchClient_OnJoinedChannel;
                twitchClient.OnLeftChannel += TwitchClient_OnLeftChannel;
                twitchClient.OnChatCommandReceived += TwitchClient_OnChatCommandReceived;
                twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
                twitchClient.Connect();

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to initialize Twitch client.");
                return false;
            }
        }

        public void JoinChannel(string channelName)
        {
            if (twitchClient == null)
                return;
            if (twitchClient.JoinedChannels.Any(c => c.Channel == channelName))
                return;
            twitchClient.JoinChannel(channelName);
        }

        public void LeaveChannel(string channelName)
        {
            if (twitchClient == null)
                return;
            var channel = twitchClient.JoinedChannels.FirstOrDefault(c => c.Channel == channelName);
            if (channel != null)
            {
                twitchClient.LeaveChannel(channel);
            }
        }

        public void RemoveChannel(string channelName)
        {
            var channel = userConfigDomain.Config!.TwitchConfig.Channels.FirstOrDefault(c => c.Name == channelName);
            if (channel != null)
            {
                userConfigDomain.Config.TwitchConfig.Channels.Remove(channel);
                userConfigDomain.SaveUserConfig();
            }
        }

        public void RemoveSong(string key)
        {
            var song = userConfigDomain.Config!.TwitchConfig.Songs.FirstOrDefault(s => s.Key == key);
            if (song != null)
            {
                userConfigDomain.Config.TwitchConfig.Songs.Remove(song);
                userConfigDomain.SaveUserConfig();
            }
        }

        #region Helper methods

        private void TwitchClient_OnConnectionError(object? sender, OnConnectionErrorArgs e)
        {
            logger.LogError("Twitch client connection error: {error}", e.Error.ToString());
        }

        private void TwitchClient_OnConnected(object? sender, OnConnectedArgs e)
        {
            logger.LogInformation("Twitch client connected to {username}", e.BotUsername);
            OnConnected?.Invoke(this, e);
        }

        private void TwitchClient_OnJoinedChannel(object? sender, OnJoinedChannelArgs e)
        {
            logger.LogInformation("Joined channel: {channel}", e.Channel);
            OnJoinedChannel?.Invoke(this, e);
        }

        private void TwitchClient_OnLeftChannel(object? sender, OnLeftChannelArgs e)
        {
            logger.LogInformation("Left channel: {channel}", e.Channel);
            OnLeftChannel?.Invoke(this, e);
        }

        private void TwitchClient_OnMessageReceived(object? sender, OnMessageReceivedArgs e)
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

        private void TwitchClient_OnChatCommandReceived(object? sender, OnChatCommandReceivedArgs e)
        {
            if (e.Command.ChatMessage.Message.StartsWith("! ")) return;
            if (e.Command.ArgumentsAsList.Count == 0) return;
            Console.WriteLine($"Received command {e.Command} with parameter {e.Command.ArgumentsAsList[0]}");
            var eventArgs = new SongRequestEventArgs
            {
                ChannelName = e.Command.ChatMessage.Channel,
                Key = e.Command.ArgumentsAsList[0]
            };
            OnBsrKeyReceived?.Invoke(sender, eventArgs);
        }

        #endregion
    }
}
