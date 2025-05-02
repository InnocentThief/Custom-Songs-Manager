using CSM.Business.Core.Twitch;
using TwitchLib.Client.Events;

namespace CSM.Business.Interfaces
{
    internal interface ITwitchChannelService
    {
        event EventHandler<OnJoinedChannelArgs> OnJoinedChannel;
        event EventHandler<OnLeftChannelArgs> OnLeftChannel;
        event EventHandler<SongRequestEventArgs> OnBsrKeyReceived;

        bool CheckChannelIsJoined(string channelName);
        bool Initialize();
        void JoinChannel(string channelName);
        void LeaveChannel(string channelName);
    }
}
