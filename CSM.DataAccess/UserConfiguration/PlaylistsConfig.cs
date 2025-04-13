namespace CSM.DataAccess.UserConfiguration
{
    internal class PlaylistsConfig
    {
        public bool Available { get; set; } = true;
        public PlaylistPath PlaylistPath { get; set; } = new();
        public PlaylistsSourceAvailability SourceAvailability { get; set; } = PlaylistsSourceAvailability.CustomLevels | PlaylistsSourceAvailability.Playlists | PlaylistsSourceAvailability.BeatSaberFavourites | PlaylistsSourceAvailability.SongSearch | PlaylistsSourceAvailability.SongSuggest;

        public PlaylistsSourceAvailability DefaultSource { get; set; }
    }
}
