using CSM.DataAccess.Entities.Offline;
using CSM.UiLogic.Workspaces.Playlists;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.TwitchIntegration
{
    public class ReceivedBeatmapViewModel
    {
        private bool canAddToPlaylist;

        #region Public Properties

        public ReceivedBeatmap ReceivedBeatmap { get; }

        public string ChannelName => ReceivedBeatmap.ChannelName;

        public DateTime ReceivedAt => ReceivedBeatmap.ReceivedAt;

        public string Key => ReceivedBeatmap.Key;

        public int BsrKeyHex
        {
            get
            {
                int.TryParse(Key, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int result);
                return result;
            }
        }

        public string Hash => ReceivedBeatmap.Hash;

        public string LevelId => $"custom_level_{ReceivedBeatmap.Hash}";

        public string SongName => ReceivedBeatmap.SongName;

        public string LevelAuthorName => ReceivedBeatmap.LevelAuthorName;

        public string SongAuthorName => ReceivedBeatmap.SongAuthorName;

        public RelayCommand AddToPlaylistCommand { get; }

        public RelayCommand DeleteSongCommand { get; }

        #endregion

        public event EventHandler<AddSongToPlaylistEventArgs> AddSongToPlaylistEvent;

        public event EventHandler DeleteSongEvent;

        public ReceivedBeatmapViewModel(ReceivedBeatmap receivedBeatMap)
        {
            ReceivedBeatmap = receivedBeatMap;

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
