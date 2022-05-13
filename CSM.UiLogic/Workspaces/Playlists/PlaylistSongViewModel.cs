using CSM.DataAccess.Entities.Offline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.Playlists
{
    public class PlaylistSongViewModel
    {
        private PlaylistSong playlistSong;

        public string SongName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(playlistSong.SongName)) return playlistSong.SongName;
                return "NA (Try to use Tools to fix this issue)";
            }
        }

        public PlaylistSongViewModel(PlaylistSong playlistSong)
        {
            this.playlistSong = playlistSong;
        }
    }
}
