using System.Text.Json;
using CSM.Business.Interfaces;
using CSM.DataAccess;
using CSM.DataAccess.Playlists;
using Microsoft.Extensions.Logging;
using Settings;
using SongSuggestNS;

namespace CSM.Business.Core
{
    internal class SongSuggestDomain(IUserConfigDomain userConfigDomain, ILogger<SongSuggestDomain> logger) : ISongSuggestDomain
    {
        #region Private fields

        private SongSuggest? songSuggest;

        private readonly IUserConfigDomain userConfigDomain = userConfigDomain;

        #endregion

        public async Task InitializeAsync()
        {
            await Task.Run(InitializeSongSuggest);
        }

        public async Task GenerateSongSuggestionsAsync(string? playerId = null)
        {
            if (songSuggest == null)
            {
                logger.LogError("SongSuggest is not initialized. Please call InitializeAsync() first.");
                return;
            }

            try
            {
                var songSuggestSettings = GetSongSuggestSettings(playerId);
                await Task.Run(() => songSuggest.GenerateSongSuggestions(songSuggestSettings));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error generating song suggestions.");
                throw;
            }
        }

        public async Task<Playlist?> GetPlaylistAsync()
        {
            if (songSuggest == null)
                throw new InvalidOperationException("SongSuggest is not initialized.");

            var playlistFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Custom Songs Manager", "SongSuggest", "Playlists", "Song Suggest.bplist");

            if (!File.Exists(playlistFile))
                return null;

            var content = await File.ReadAllTextAsync(playlistFile);
            return JsonSerializer.Deserialize<Playlist>(content, JsonSerializerHelper.CreateDefaultSerializerOptions());
        }

        public string? GetPlaylistPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Custom Songs Manager", "SongSuggest", "Playlists", "Song Suggest.bplist");
        }

        #region Helper methods

        private void InitializeSongSuggest()
        {
            if (songSuggest == null)
            {
                var settings = new CoreSettings()
                {
                    FilePathSettings = new FilePathSettings(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Custom Songs Manager", "SongSuggest")),
                    Log = new SongSuggestLogger(logger),
                    UserID = userConfigDomain.Config?.LeaderboardsConfig.DefaultLeaderboard == DataAccess.UserConfiguration.LeaderboardType.ScoreSaber ? userConfigDomain.Config?.LeaderboardsConfig.ScoreSaberUserId : userConfigDomain.Config?.LeaderboardsConfig.BeatLeaderUserId,
                    UseScoreSaberLeaderboard = userConfigDomain.Config?.LeaderboardsConfig.UseScoreSaberLeaderboard ?? false,
                    UseBeatLeaderLeaderboard = userConfigDomain.Config?.LeaderboardsConfig.UseBeatLeaderLeaderboard ?? false,
                };
                songSuggest = new SongSuggest(settings);
            }
        }

        private SongSuggestSettings GetSongSuggestSettings(string? playerId)
        {
            string? playerIdToUse = playerId;
            if (string.IsNullOrWhiteSpace(playerIdToUse))
            {
                playerIdToUse = userConfigDomain.Config?.LeaderboardsConfig.DefaultLeaderboard == DataAccess.UserConfiguration.LeaderboardType.ScoreSaber ? userConfigDomain.Config?.LeaderboardsConfig.ScoreSaberUserId : userConfigDomain.Config?.LeaderboardsConfig.BeatLeaderUserId;
            }

            return new SongSuggestSettings
            {
                SuggestionName = userConfigDomain.Config?.SongSuggestSettings.SuggestionName ?? string.Empty,
                PlayerID = playerIdToUse,
                IgnorePlayedAll = userConfigDomain.Config?.SongSuggestSettings.IgnorePlayedAll ?? false,
                IgnorePlayedDays = userConfigDomain.Config?.SongSuggestSettings.IgnorePlayedDays ?? 14,
                IgnoreNonImproveable = userConfigDomain.Config?.SongSuggestSettings.IgnoreNonImprovable ?? true,
                UseLikedSongs = userConfigDomain.Config?.SongSuggestSettings.UseLikedSongs ?? false,
                FillLikedSongs = userConfigDomain.Config?.SongSuggestSettings.FillLikedSongs ?? true,
                UseLocalScores = userConfigDomain.Config?.SongSuggestSettings.UseLocalScores ?? false,
                ExtraSongs = userConfigDomain.Config?.SongSuggestSettings.ExtraSongs ?? 15,
                PlaylistLength = userConfigDomain.Config?.SongSuggestSettings.PlaylistLength ?? 50,
                Leaderboard = userConfigDomain.Config?.LeaderboardsConfig.DefaultLeaderboard == DataAccess.UserConfiguration.LeaderboardType.ScoreSaber ? Actions.LeaderboardType.ScoreSaber : Actions.LeaderboardType.BeatLeader,
                OriginSongCount = userConfigDomain.Config?.SongSuggestSettings.OriginSongCount ?? 50,
                FilterSettings = new FilterSettings
                {
                    modifierStyle = userConfigDomain.Config?.SongSuggestSettings.FilterSettings.ModifierStyle ?? 100.0,
                    modifierOverweight = userConfigDomain.Config?.SongSuggestSettings.FilterSettings.ModifierOverweight ?? 81.0,
                },
                PlaylistSettings = new PlaylistSettings
                {
                    title = userConfigDomain.Config?.SongSuggestSettings.PlaylistSettings.Title ?? string.Empty,
                    author = userConfigDomain.Config?.SongSuggestSettings.PlaylistSettings.Author ?? string.Empty,
                    fileName = userConfigDomain.Config?.SongSuggestSettings.PlaylistSettings.FileName ?? string.Empty,
                    description = userConfigDomain.Config?.SongSuggestSettings.PlaylistSettings.Description ?? string.Empty,
                    syncURL = userConfigDomain.Config?.SongSuggestSettings.PlaylistSettings.SyncUrl ?? string.Empty,
                    image = userConfigDomain.Config?.SongSuggestSettings.PlaylistSettings.Image ?? string.Empty,
                },
            };
        }

        #endregion
    }
}
