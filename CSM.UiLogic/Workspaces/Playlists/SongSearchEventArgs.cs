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

        public int PageIndex { get; }

        /// <summary>
        /// Gets whether the searchstring is a bsr key.
        /// </summary>
        public bool IsKey { get; }

        /// <summary>
        /// Initializes a new <see cref="SongSearchEventArgs"/>.
        /// </summary>
        public SongSearchEventArgs(string searchString, int pageIndex, bool isKey)
        {
            SearchString = searchString;
            PageIndex = pageIndex;
            IsKey = isKey;
        }
    }
}