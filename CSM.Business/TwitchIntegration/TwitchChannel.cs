using System;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;

namespace CSM.Business.TwitchIntegration
{
    public class TwitchChannel
    {
        private TwitchClient twitchClient;

        public Guid ChannelId { get; }

        public string ChannelName { get; }

        public event EventHandler<OnConnectedArgs> OnConnected;

        public event EventHandler<OnDisconnectedEventArgs> OnDisconnected;

        public event EventHandler<OnChatCommandReceivedArgs> OnChatCommandReceived;

        public TwitchChannel(Guid channelId, string channelName)
        {
            ChannelId = channelId;
            ChannelName = channelName;
        }

        public void Initialize(ConnectionCredentials connectionCredentials)
        {
            twitchClient = new TwitchClient();
            twitchClient.Initialize(connectionCredentials, ChannelName);
            twitchClient.OnConnected += TwitchClient_OnConnected;
            twitchClient.OnDisconnected += TwitchClient_OnDisconnected;
            twitchClient.OnChatCommandReceived += TwitchClient_OnChatCommandReceived;
            twitchClient.Connect();
        }

        public void CleanUp()
        {
            twitchClient.Disconnect();
            twitchClient.OnConnected -= TwitchClient_OnConnected;
            twitchClient.OnDisconnected -= TwitchClient_OnDisconnected;
            twitchClient.OnChatCommandReceived -= TwitchClient_OnChatCommandReceived;
        }

        private void TwitchClient_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            OnChatCommandReceived?.Invoke(this, e);
        }

        private void TwitchClient_OnDisconnected(object sender, OnDisconnectedEventArgs e)
        {
            twitchClient.SendMessage(ChannelName, $"Custom Songs Manager disconnected from the chat.");
        }

        private void TwitchClient_OnConnected(object sender, OnConnectedArgs e)
        {
            twitchClient.SendMessage(ChannelName, $"Custom Songs Manager connected to the chat.");
        }
    }
}