using CSM.Business.Core.Twitch;
using CSM.DataAccess.Twitch;
using TwitchLib.Client.Events;

namespace CSM.Business.Interfaces
{
    internal interface ITwitchChannelService
    {
        TwitchSongs? TwitchSongs { get; }

        event EventHandler<OnJoinedChannelArgs> OnJoinedChannel;
        event EventHandler<OnLeftChannelArgs> OnLeftChannel;
        event EventHandler<SongRequestEventArgs> OnBsrKeyReceived;
        event EventHandler<OnConnectedArgs>? OnConnected;

        void AddChannel(string channelName);
        Task AddSongAsync(string key, string channelName, DateTime receivedAt);
        bool CheckChannelIsJoined(string channelName);
        Task ClearSongHistoryAsync();
        Task<bool> Initialize();
        void JoinChannel(string channelName);
        void LeaveChannel(string channelName);
        void RemoveChannel(string channelName);
        Task RemoveSongAsync(string key);
    }
}
