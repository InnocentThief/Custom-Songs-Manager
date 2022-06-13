using System;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Events;
using TwitchLib.Communication.Models;

namespace CSM.Business.TwitchIntegration
{
    public class TwitchChannel
    {
        private TwitchClient twitchClient;

        public Guid ChannelId { get; }

        public string ChannelName { get; }

        public event EventHandler<OnChatCommandReceivedArgs> OnChatCommandReceived;

        public TwitchChannel(Guid channelId, string channelName)
        {
            ChannelId = channelId;
            ChannelName = channelName;
        }

        //public void Initialize(ConnectionCredentials connectionCredentials)
        //{
        //    //var clientOptions = new ClientOptions
        //    //{
        //    //    MessagesAllowedInPeriod = 750,
        //    //    ThrottlingPeriod = TimeSpan.FromSeconds(30)
        //    //};
        //    //WebSocketClient customClient = new WebSocketClient(clientOptions);
        //    //twitchClient = new TwitchClient(customClient);
        //    //twitchClient.Initialize(connectionCredentials, ChannelName);
        //    //twitchClient.OnLog += TwitchClient_OnLog;
        //    //twitchClient.OnJoinedChannel += TwitchClient_OnJoinedChannel;
        //    //twitchClient.OnChatCommandReceived += TwitchClient_OnChatCommandReceived;
        //    //twitchClient.OnConnected += TwitchClient_OnConnected;
            
        //    ////twitchClient.OnDisconnected += TwitchClient_OnDisconnected;
        //    ////twitchClient.OnChatCommandReceived += TwitchClient_OnChatCommandReceived;


        //    //twitchClient.Connect();
        //}

        //#region Helper methods

        //private void TwitchClient_OnLog(object sender, OnLogArgs e)
        //{
        //    Console.WriteLine($"{e.DateTime.ToString()}: {e.BotUsername} - {e.Data}");
        //}

        //private void TwitchClient_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        //{
        //    Console.WriteLine($"Custom Songs Manager connected to {e.Channel}");
        //    twitchClient.SendMessage(ChannelName, $"Custom Songs Manager connected to {e.Channel}");
        //}

        //private void TwitchClient_OnConnected(object sender, OnConnectedArgs e)
        //{
        //    //Console.WriteLine($"Connected to {e.AutoJoinChannel}");
        //    //twitchClient.SendMessage(ChannelName, $"Custom Songs Manager connected to {e.AutoJoinChannel}");
        //}

        //private void TwitchClient_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        //{


        //    OnChatCommandReceived?.Invoke(this, e);
        //}

        //#endregion







        //public void CleanUp()
        //{
        //    //    twitchClient.Disconnect();
        //    //    twitchClient.OnConnected -= TwitchClient_OnConnected;
        //    //    twitchClient.OnDisconnected -= TwitchClient_OnDisconnected;
        //    //    twitchClient.OnChatCommandReceived -= TwitchClient_OnChatCommandReceived;
        //}



        //private void TwitchClient_OnDisconnected(object sender, OnDisconnectedEventArgs e)
        //{
        //    //twitchClient.SendMessage(ChannelName, $"Custom Songs Manager disconnected from the chat.");
        //}


    }
}