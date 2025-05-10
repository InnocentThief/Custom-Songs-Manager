using System.Text.Json;
using CSM.Business.Core.Twitch;
using CSM.Business.Interfaces;
using CSM.DataAccess;
using CSM.DataAccess.Twitch;
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
        private string twitchSongsFilePath = string.Empty;
        static SemaphoreSlim semaphore = new(1); // Allow up to 3 threads

        const string beatsaverUri = "https://beatsaver.com/maps/";

        public TwitchSongs? TwitchSongs { get; private set; }

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

        public async Task AddSongAsync(string key, string channelName, DateTime receivedAt)
        {
            await semaphore.WaitAsync();

            try
            {
                if (!File.Exists(twitchSongsFilePath))
                {
                    TwitchSongs = new TwitchSongs
                    {
                        Songs = [],
                    };
                    var newContent = JsonSerializer.Serialize(TwitchSongs, JsonSerializerHelper.CreateDefaultSerializerOptions());
                    await File.WriteAllTextAsync(twitchSongsFilePath, newContent);
                }
                else
                {
                    if (TwitchSongs == null)
                    {
                        var content = await File.ReadAllTextAsync(twitchSongsFilePath);
                        TwitchSongs = JsonSerializer.Deserialize<TwitchSongs>(content, JsonSerializerHelper.CreateDefaultSerializerOptions());
                    }
                }

                if (TwitchSongs == null)
                    return;

                if (TwitchSongs.Songs.Find(s => s.ChannelName == channelName && s.Key == key) != null)
                    return;

                TwitchSongs.Songs.Add(new TwitchSong { Key = key, ChannelName = channelName, ReceivedAt = receivedAt });
                var json = JsonSerializer.Serialize(TwitchSongs, JsonSerializerHelper.CreateDefaultSerializerOptions());
                await File.WriteAllTextAsync(twitchSongsFilePath, json);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to add song to Twitch songs file.");
            }
            finally
            {
                semaphore.Release();
            }
        }

        public bool CheckChannelIsJoined(string channelName)
        {
            return twitchClient != null && twitchClient.JoinedChannels.Any(c => c.Channel == channelName);
        }

        public async Task ClearSongHistoryAsync()
        {
            if (TwitchSongs == null)
                return;
            TwitchSongs.Songs.Clear();
            var json = JsonSerializer.Serialize(TwitchSongs, JsonSerializerHelper.CreateDefaultSerializerOptions());
            await File.WriteAllTextAsync(twitchSongsFilePath, json);
        }

        public async Task<bool> Initialize()
        {
            try
            {
                if (twitchClient != null)
                    return true;

                twitchSongsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Custom Songs Manager", "TwitchSongs.json");

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

                if (!File.Exists(twitchSongsFilePath))
                {
                    TwitchSongs = new TwitchSongs
                    {
                        Songs = [],
                    };
                    var newContent = JsonSerializer.Serialize(TwitchSongs, JsonSerializerHelper.CreateDefaultSerializerOptions());
                    await File.WriteAllTextAsync(twitchSongsFilePath, newContent);
                }
                else
                {
                    if (TwitchSongs == null)
                    {
                        var content = await File.ReadAllTextAsync(twitchSongsFilePath);
                        TwitchSongs = JsonSerializer.Deserialize<TwitchSongs>(content, JsonSerializerHelper.CreateDefaultSerializerOptions());
                    }
                }

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

        public async Task RemoveSongAsync(string key)
        {
            await semaphore.WaitAsync();

            try
            {
                if (!File.Exists(twitchSongsFilePath))
                {
                    TwitchSongs = new TwitchSongs
                    {
                        Songs = [],
                    };
                    var newContent = JsonSerializer.Serialize(TwitchSongs, JsonSerializerHelper.CreateDefaultSerializerOptions());
                    await File.WriteAllTextAsync(twitchSongsFilePath, newContent);
                }
                else
                {
                    if (TwitchSongs == null)
                    {
                        var content = await File.ReadAllTextAsync(twitchSongsFilePath);
                        TwitchSongs = JsonSerializer.Deserialize<TwitchSongs>(content, JsonSerializerHelper.CreateDefaultSerializerOptions());
                    }
                }

                if (TwitchSongs == null)
                    return;

                var song = TwitchSongs.Songs.FirstOrDefault(s => s.Key == key);
                if (song != null)
                {
                    TwitchSongs.Songs.Remove(song);
                    var json = JsonSerializer.Serialize(TwitchSongs, JsonSerializerHelper.CreateDefaultSerializerOptions());
                    await File.WriteAllTextAsync(twitchSongsFilePath, json);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to remove song from Twitch songs file.");
            }
            finally
            {
                semaphore.Release();
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
