using CSM.DataAccess.Entities.Online;
using Microsoft.Toolkit.Mvvm.Input;
using System;

namespace CSM.UiLogic.Workspaces.Playlists
{
    /// <summary>
    /// Represent a favorite.
    /// </summary>
    public class FavoriteViewModel
    {
        #region Private fields

        private BeatMap beatmap;
        private bool canAddToPlaylist;

        #endregion

        #region Public Properties

        public string Key => beatmap.Id;

        public string SongName => beatmap.Metadata.SongName;

        public string LevelAuthorName => beatmap.Metadata.LevelAuthorName;

        public string SongAuthorName => beatmap.Metadata.SongAuthorName;

        public RelayCommand AddToPlaylistCommand { get; }

        #endregion

        public event EventHandler<AddSongToPlaylistEventArgs> AddSongToPlaylistEvent;

        /// <summary>
        /// Initializes a new <see cref="FavoriteViewModel"/>.
        /// </summary>
        /// <param name="beatmap">BeatMap of the favorite.</param>
        public FavoriteViewModel(BeatMap beatmap)
        {
            this.beatmap = beatmap;

            AddToPlaylistCommand = new RelayCommand(AddToPlaylist, CanAddToPlaylist);
        }

        /// <summary>
        /// Sets whether adding to playlist is available.
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
            AddSongToPlaylistEvent?.Invoke(this, new AddSongToPlaylistEventArgs { BsrKey = Key });
        }

        private bool CanAddToPlaylist()
        {
            return canAddToPlaylist;
        }

        #endregion
    }
}