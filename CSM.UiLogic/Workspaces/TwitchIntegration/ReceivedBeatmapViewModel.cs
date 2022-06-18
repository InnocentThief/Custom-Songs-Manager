using CSM.DataAccess.Entities.Offline;
using CSM.UiLogic.Workspaces.Playlists;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.TwitchIntegration
{
    public class ReceivedBeatmapViewModel
    {
        #region Private fields

        private ReceivedBeatmap receivedBeatmap;
        private bool canAddToPlaylist;

        #endregion

        #region Public Properties

        public string ChannelName => receivedBeatmap.ChannelName;

        public DateTime ReceivedAt => receivedBeatmap.ReceivedAt;

        public string Key => receivedBeatmap.Key;

        public string Hash => receivedBeatmap.Hash;

        public string LevelId => $"custom_level_{receivedBeatmap.Hash}";

        public string SongName => receivedBeatmap.SongName;

        public string LevelAuthorName => receivedBeatmap.LevelAuthorName;

        public string SongAuthorName => receivedBeatmap.SongAuthorName;

        public RelayCommand AddToPlaylistCommand { get; }

        public RelayCommand DeleteSongCommand { get; }

        #endregion

        public event EventHandler<AddSongToPlaylistEventArgs> AddSongToPlaylistEvent;

        public event EventHandler DeleteSongEvent;

        public ReceivedBeatmapViewModel(ReceivedBeatmap receivedBeatMap)
        {
            this.receivedBeatmap = receivedBeatMap;

            AddToPlaylistCommand = new RelayCommand(AddToPlaylist, CanAddToPlaylist);
            DeleteSongCommand = new RelayCommand(DeleteSong);
        }

        /// <summary>
        /// Sets whether adding the song to a playlist is available.
        /// </summary>
        /// <param name="playlistSelected">Indicates whether a playlist is selected.</param>
        public void SetCanAddToPlaylist(bool playlistSelected)
        {
            canAddToPlaylist = playlistSelected;
            AddToPlaylistCommand.NotifyCanExecuteChanged();
        }

        #region Helper methods

        private void AddToPlaylist()
        {
            var addEvent = new AddSongToPlaylistEventArgs
            {
                BsrKey = Key,
                Hash = Hash,
                LevelAuthorName = LevelAuthorName,
                LevelId = LevelId,
                SongName = SongName
            };
            AddSongToPlaylistEvent?.Invoke(this, addEvent);
        }

        private bool CanAddToPlaylist()
        {
            return canAddToPlaylist;
        }

        private void DeleteSong()
        {
            DeleteSongEvent?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
