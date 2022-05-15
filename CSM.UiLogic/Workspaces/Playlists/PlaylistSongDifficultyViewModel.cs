using CSM.DataAccess.Entities.Offline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.Playlists
{
    public class PlaylistSongDifficultyViewModel
    {
        private PlaylistSongDifficulty playlistSongDifficulty;

        public string Characteristic => playlistSongDifficulty.Characteristic;

        public string Name => playlistSongDifficulty.Name;

        public PlaylistSongDifficultyViewModel(PlaylistSongDifficulty playlistSongDifficulty)
        {
            this.playlistSongDifficulty = playlistSongDifficulty;
        }
    }
}
