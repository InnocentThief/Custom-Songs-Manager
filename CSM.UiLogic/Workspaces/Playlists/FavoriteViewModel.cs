using CSM.DataAccess.Entities.Online;

namespace CSM.UiLogic.Workspaces.Playlists
{
    public class FavoriteViewModel
    {
        private BeatMap beatmap;

        public string SongName => beatmap.Metadata.SongName;

        public string LevelAuthorName => beatmap.Metadata.LevelAuthorName;

        public string SongAuthorName => beatmap.Metadata.SongAuthorName;

        public FavoriteViewModel(BeatMap beatmap)
        {
            this.beatmap = beatmap;
        }
    }
}