using CSM.DataAccess.Playlists;

namespace CSM.Business.Interfaces
{
    internal interface ISongSuggestDomain
    {
        Task InitializeAsync();

        Task GenerateSongSuggestionsAsync(string? playerId = null);

        Task<Playlist?> GetPlaylistAsync();

        string? GetPlaylistPath();
    }
}
