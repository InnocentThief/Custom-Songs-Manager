using CSM.DataAccess.Entities.Online;
using Microsoft.Toolkit.Mvvm.Input;
using System;

namespace CSM.UiLogic.Workspaces.Playlists
{
    /// <summary>
    /// Represents one result of a song search.
    /// </summary>
    public class SearchedSongViewModel
    {
        private bool canAddToPlaylist;

        #region Public Properties

        public BeatMap BeatMap { get; }

        public string Key => BeatMap.Id;

        public string SongName => BeatMap.Metadata.SongName;

        public string LevelAuthorName => BeatMap.Metadata.LevelAuthorName;

        public string SongAuthorName => BeatMap.Metadata.SongAuthorName;

        public int Upvotes => BeatMap.Stats.Upvotes;

        public int Downvotes => BeatMap.Stats.Downvotes;

        public RelayCommand AddToPlaylistCommand { get; }

        #endregion

        /// <summary>
        /// Occurs on song add event.
        /// </summary>
        public event EventHandler<AddSongToPlaylistEventArgs> AddSongToPlaylistEvent;

        /// <summary>
        /// Initializes a new <see cref="SearchedSongViewModel"/>.
        /// </summary>
        /// <param name="beatmap">The beat map of the song.</param>
        public SearchedSongViewModel(BeatMap beatmap)
        {
            BeatMap = beatmap;

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