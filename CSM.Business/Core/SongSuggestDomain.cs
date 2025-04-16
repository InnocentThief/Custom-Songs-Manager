using CSM.Business.Interfaces;
using CSM.DataAccess.Playlists;
using Settings;
using SongSuggestNS;
using System.Text.Json;

namespace CSM.Business.Core
{
    internal class SongSuggestDomain(IUserConfigDomain userConfigDomain) : ISongSuggestDomain
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
                throw new InvalidOperationException("SongSuggest is not initialized.");

            try
            {
                var songSuggestSettings = GetSongSuggestSettings(playerId);
                await Task.Run(() => songSuggest.GenerateSongSuggestions(songSuggestSettings));
            }
            catch (Exception ex)
            {

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
            return JsonSerializer.Deserialize<Playlist>(content);
        }

        #region Helper methods

        private void InitializeSongSuggest()
        {
            if (songSuggest == null)
            {
                var settings = new CoreSettings()
                {
                    FilePathSettings = new FilePathSettings(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Custom Songs Manager", "SongSuggest")),
                    UserID = userConfigDomain.Config?.SongSuggestConfig.DefaultLeaderboard == DataAccess.UserConfiguration.LeaderboardType.ScoreSaber ? userConfigDomain.Config?.SongSuggestConfig.ScoreSaberUserId : userConfigDomain.Config?.SongSuggestConfig.BeatLeaderUserId,
                    UseScoreSaberLeaderboard = userConfigDomain.Config?.SongSuggestConfig.UseScoreSaberLeaderboard ?? false,
                    UseBeatLeaderLeaderboard = userConfigDomain.Config?.SongSuggestConfig.UseBeatLeaderLeaderboard ?? false,
                };
                songSuggest = new SongSuggest(settings);
            }
        }

        private SongSuggestSettings GetSongSuggestSettings(string? playerId)
        {
            string? playerIdToUse = playerId;
            if (string.IsNullOrWhiteSpace(playerIdToUse))
            {
                playerIdToUse = userConfigDomain.Config?.SongSuggestConfig.DefaultLeaderboard == DataAccess.UserConfiguration.LeaderboardType.ScoreSaber ? userConfigDomain.Config?.SongSuggestConfig.ScoreSaberUserId : userConfigDomain.Config?.SongSuggestConfig.BeatLeaderUserId;
            }

            return new SongSuggestSettings
            {
                SuggestionName = userConfigDomain.Config?.SongSuggestConfig.SongSuggestSettings.SuggestionName ?? string.Empty,
                PlayerID = playerIdToUse,
                IgnorePlayedAll = userConfigDomain.Config?.SongSuggestConfig.SongSuggestSettings.IgnorePlayedAll ?? false,
                IgnorePlayedDays = userConfigDomain.Config?.SongSuggestConfig.SongSuggestSettings.IgnorePlayedDays ?? 14,
                IgnoreNonImproveable = userConfigDomain.Config?.SongSuggestConfig.SongSuggestSettings.IgnoreNonImprovable ?? true,
                RequiredMatches = userConfigDomain.Config?.SongSuggestConfig.SongSuggestSettings.RequiredMatches ?? 90,
                UseLikedSongs = userConfigDomain.Config?.SongSuggestConfig.SongSuggestSettings.UseLikedSongs ?? false,
                FillLikedSongs = userConfigDomain.Config?.SongSuggestConfig.SongSuggestSettings.FillLikedSongs ?? true,
                UseLocalScores = userConfigDomain.Config?.SongSuggestConfig.SongSuggestSettings.UseLocalScores ?? false,
                ExtraSongs = userConfigDomain.Config?.SongSuggestConfig.SongSuggestSettings.ExtraSongs ?? 15,
                PlaylistLength = userConfigDomain.Config?.SongSuggestConfig.SongSuggestSettings.PlaylistLength ?? 50,
                Leaderboard = userConfigDomain.Config?.SongSuggestConfig.DefaultLeaderboard == DataAccess.UserConfiguration.LeaderboardType.ScoreSaber ? Actions.LeaderboardType.ScoreSaber : Actions.LeaderboardType.BeatLeader,
                OriginSongCount = userConfigDomain.Config?.SongSuggestConfig.SongSuggestSettings.OriginSongCount ?? 50,
                FilterSettings = new FilterSettings
                {
                    modifierStyle = userConfigDomain.Config?.SongSuggestConfig.SongSuggestSettings.FilterSettings.ModifierStyle ?? 100.0,
                    modifierOverweight = userConfigDomain.Config?.SongSuggestConfig.SongSuggestSettings.FilterSettings.ModifierOverweight ?? 81.0,
                },
                PlaylistSettings = new PlaylistSettings
                {
                    title = userConfigDomain.Config?.SongSuggestConfig.SongSuggestSettings.PlaylistSettings.Title ?? string.Empty,
                    author = userConfigDomain.Config?.SongSuggestConfig.SongSuggestSettings.PlaylistSettings.Author ?? string.Empty,
                    fileName = userConfigDomain.Config?.SongSuggestConfig.SongSuggestSettings.PlaylistSettings.FileName ?? string.Empty,
                    description = userConfigDomain.Config?.SongSuggestConfig.SongSuggestSettings.PlaylistSettings.Description ?? string.Empty,
                    syncURL = userConfigDomain.Config?.SongSuggestConfig.SongSuggestSettings.PlaylistSettings.SyncUrl ?? string.Empty,
                    image = userConfigDomain.Config?.SongSuggestConfig.SongSuggestSettings.PlaylistSettings.Image ?? string.Empty,
                },
            };
        }

        #endregion
    }
}
