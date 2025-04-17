using CSM.DataAccess.Playlists;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using System.Globalization;

namespace CSM.UiLogic.ViewModels.Common.Playlists
{
    internal class PlaylistSongViewModel(IServiceLocator serviceLocator, Song song)
        : BaseViewModel(serviceLocator)
    {
        private Song song = song;

        #region Properties

        public string Hash => song.Hash.ToLower();

        public string BsrKey => song.Key ?? string.Empty;

        public int BsrKeyHey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(song.Key))
                    return 0;
                int.TryParse(BsrKey, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int result);
                return result;
            }
        }

        public string SongName => song.SongName;

        public string? LevelAuthorName => song.LevelAuthorName;

        #endregion

    }

}
