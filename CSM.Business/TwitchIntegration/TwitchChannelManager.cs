using CSM.Business.TwitchIntegration.TwitchConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchLib.Api;
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
        //private TwitchAPI twitchAPI;
        //private List<TwitchChannel> connectedChannels;
        //private ConnectionCredentials connectionCredentials;


        public bool IsConnected => twitchClient != null && twitchClient.IsConnected;



        public event EventHandler<OnJoinedChannelArgs> OnJoinedChannel;

        public event EventHandler<OnChatCommandReceivedArgs> OnChatCommandReceived;

        //public async Task InitializeAsync()
        //{

        //}

        /// <summary>
        /// Adds a channel with the given id and name to the connected channels list.
        /// </summary>
        /// <param name="channelId">Unique identifier of the channel.</param>
        /// <param name="channelName">Name of the channel.</param>
        public void AddChannel(string channelName)
        {
            if (!twitchClient.JoinedChannels.Any(c => c.Channel == channelName))
            {
                twitchClient.JoinChannel(channelName);
            }

            //if (!connectedChannels.Any(c => c.ChannelId == channelId))
            //{
            //    var channel = new TwitchChannel(channelId, name);
            //    if (connectionCredentials == null) connectionCredentials = new ConnectionCredentials("InnocentThief", TwitchConfigManager.Instance.Config.AccessToken);
            //    channel.Initialize(connectionCredentials);
            //    channel.OnChatCommandReceived += Channel_OnChatCommandReceived;
            //}
        }

        /// <summary>
        /// Removes the channel with the given id from the list of connected channels.
        /// </summary>
        /// <param name="channelName">The name of the channel to remove.</param>
        public void RemoveChannel(string channelName)
        {
            var channel = twitchClient.JoinedChannels.FirstOrDefault(c => c.Channel == channelName);
            if (channel != null)
            {
                twitchClient.LeaveChannel(channel);
            }

            //if (twitchClient.JoinedChannels.Any(c => c.Channel == channelName))
            //{
            //    twitchClient.LeaveChannel()
            //}

            //var connectedChannel = connectedChannels.SingleOrDefault(c => c.ChannelId == channelId);
            //if (connectedChannel != null)
            //{
            //    //connectedChannel.OnChatCommandReceived -= Channel_OnChatCommandReceived;
            //    //connectedChannel.CleanUp();
            //    //connectedChannels.Remove(connectedChannel);
            //}
        }

        public bool CheckChannelIsConnected(string channelName)
        {
            return twitchClient.JoinedChannels.Any(c => c.Channel == channelName);
        }

        #region Helper methods

        private void Setup()
        {
            // TODO: Validate token
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
            twitchClient.OnChatCommandReceived += TwitchClient_OnChatCommandReceived;
            twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            twitchClient.OnConnected += TwitchClient_OnConnected;

            twitchClient.Connect();

            //twitchAPI = new TwitchAPI();
            //twitchAPI.Settings.ClientId = "mf66rq31qva9bv7dit1jygdjs39loa";
            //twitchAPI.Settings.AccessToken = TwitchConfigManager.Instance.Config.AccessToken;

            //connectedChannels = new List<TwitchChannel>();
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

        private void TwitchClient_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            OnChatCommandReceived?.Invoke(sender, e);
        }

        private void TwitchClient_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            throw new NotImplementedException();
        }

        private void TwitchClient_OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine($"Custom Songs Manager connected to Twitch");
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