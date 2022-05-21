using CSM.DataAccess.Entities.Offline;
using CSM.UiLogic.Workspaces.Playlists;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Linq;

namespace CSM.UiLogic.Workspaces.CustomLevels
{
    /// <summary>
    /// Represents one custom level.
    /// </summary>
    public class CustomLevelViewModel
    {
        #region Private fields

        private readonly CustomLevel customLevel;
        private bool canAddToPlaylist;

        #endregion

        #region Public Properties

        public string BsrKey => customLevel.BsrKey;

        public string SongName => customLevel.SongName;

        public string SongSubName => customLevel.SongSubName;

        public string LevelAuthorName => customLevel.LevelAuthorName;

        public string SongAuthorName => customLevel.SongAuthorName;

        public bool Easy => customLevel.DifficultiySets.SelectMany(ds => ds.Difficulties).Any(d => d.DifficultyRank == 1);

        public bool Normal => customLevel.DifficultiySets.SelectMany(ds => ds.Difficulties).Any(d => d.DifficultyRank == 3);

        public bool Hard => customLevel.DifficultiySets.SelectMany(ds => ds.Difficulties).Any(d => d.DifficultyRank == 5);

        public bool Expert => customLevel.DifficultiySets.SelectMany(ds => ds.Difficulties).Any(d => d.DifficultyRank == 7);

        public bool ExpertPlus => customLevel.DifficultiySets.SelectMany(ds => ds.Difficulties).Any(d => d.DifficultyRank == 9);

        public DateTime ChangeDate => customLevel.ChangeDate;

        public string Path => customLevel.Path;

        public RelayCommand AddToPlaylistCommand { get; }

        #endregion

        public event EventHandler<AddSongToPlaylistEventArgs> AddSongToPlaylistEvent;

        /// <summary>
        /// Initializes a new <see cref="CustomLevelViewModel"/>.
        /// </summary>
        /// <param name="customLevel">The <see cref="CustomLevel"/>.</param>
        public CustomLevelViewModel(CustomLevel customLevel)
        {
            this.customLevel = customLevel;

            AddToPlaylistCommand = new RelayCommand(AddToPlaylist, CanAddToPlaylist);
        }

        public void SetCanAddToPlaylist(bool playlistSelected)
        {
            canAddToPlaylist = playlistSelected;
            AddToPlaylistCommand.NotifyCanExecuteChanged();
        }

        #region Helper methods

        private void AddToPlaylist()
        {
            AddSongToPlaylistEvent?.Invoke(this, new AddSongToPlaylistEventArgs { BsrKey = BsrKey });
        }

        private bool CanAddToPlaylist()
        {
            return canAddToPlaylist;
        }

        #endregion
    }
}