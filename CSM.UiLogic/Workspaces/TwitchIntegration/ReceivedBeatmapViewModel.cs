using CSM.DataAccess.Entities.Offline;
using CSM.UiLogic.Workspaces.Playlists;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Globalization;

namespace CSM.UiLogic.Workspaces.TwitchIntegration
{
    /// <summary>
    /// Represents a received beatmap from Twitch.
    /// </summary>
    public class ReceivedBeatmapViewModel
    {
        private bool canAddToPlaylist;

        #region Public Properties

        /// <summary>
        /// Gets the data for the received beatmap.
        /// </summary>
        public ReceivedBeatmap ReceivedBeatmap { get; }

        /// <summary>
        /// Gets the Twitch channel name from whitch the beatmap was requestd.
        /// </summary>
        public string ChannelName => ReceivedBeatmap.ChannelName;

        /// <summary>
        /// Gets the request time.
        /// </summary>
        public DateTime ReceivedAt => ReceivedBeatmap.ReceivedAt;

        /// <summary>
        /// Gets the bsr key for the requested beatmap.
        /// </summary>
        public string Key => ReceivedBeatmap.Key;

        /// <summary>
        /// Gets the bsr key as hex for the requested beatmap.
        /// </summary>
        public int BsrKeyHex
        {
            get
            {
                int.TryParse(Key, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int result);
                return result;
            }
        }

        /// <summary>
        /// Gets the hash of the requested beatmap.
        /// </summary>
        public string Hash => ReceivedBeatmap.Hash;

        /// <summary>
        /// Gets the level id of the requested beatmap.
        /// </summary>
        public string LevelId => $"custom_level_{ReceivedBeatmap.Hash}";

        /// <summary>
        /// Gets the song name of the requested beatmap.
        /// </summary>
        public string SongName => ReceivedBeatmap.SongName;

        /// <summary>
        /// Gets the level author name of the requested beatmap.
        /// </summary>
        public string LevelAuthorName => ReceivedBeatmap.LevelAuthorName;

        /// <summary>
        /// Gets the song author name of the requested beatmap.
        /// </summary>
        public string SongAuthorName => ReceivedBeatmap.SongAuthorName;

        /// <summary>
        /// Command used to add the current song to the selected playlist.
        /// </summary>
        public RelayCommand AddToPlaylistCommand { get; }

        /// <summary>
        /// Command used to delete the current song from the received beatmaps list.
        /// </summary>
        public RelayCommand DeleteSongCommand { get; }

        #endregion

        /// <summary>
        /// Occurs on add song to playlist.
        /// </summary>
        public event EventHandler<AddSongToPlaylistEventArgs> AddSongToPlaylistEvent;

        /// <summary>
        /// Occurs on song delete.
        /// </summary>
        public event EventHandler DeleteSongEvent;

        /// <summary>
        /// Initializes a new <see cref="ReceivedBeatmapViewModel"/>.
        /// </summary>
        /// <param name="receivedBeatMap"></param>
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