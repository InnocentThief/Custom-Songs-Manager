namespace CSM.DataAccess.UserConfiguration
{
    [Flags]
    internal enum PlaylistsSourceAvailability : byte
    {
        None = 0,
        CustomLevels = 1,
        Playlists = 2,
        BeatSaberFavourites = 4,
        SongSearch = 8,
        SongSuggest = 16,
        Twitch = 32,
        BeatLeader = 64,
        ScoreSaber = 128,
    }
}
