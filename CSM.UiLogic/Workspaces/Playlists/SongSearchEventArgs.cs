using System;

namespace CSM.UiLogic.Workspaces.Playlists
{
    /// <summary>
    /// EventArgs on song search, containing the search string.
    /// </summary>
    public class SongSearchEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the query for the search.
        /// </summary>
        public string SearchString { get; }

        /// <summary>
        /// Initializes a new <see cref="SongSearchEventArgs"/>.
        /// </summary>
        /// <param name="searchString">The query used for the search.</param>
        public SongSearchEventArgs(string searchString)
        {
            SearchString = searchString;
        }
    }
}