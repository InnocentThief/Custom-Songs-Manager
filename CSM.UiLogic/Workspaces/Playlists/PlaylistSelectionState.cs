using System;

namespace CSM.UiLogic.Workspaces.Playlists
{
    /// <summary>
    /// Helper class that contains the state of playlist selections.
    /// </summary>
    public class PlaylistSelectionState
    {
        /// <summary>
        /// Gets or sets whether a playlist is selected.
        /// </summary>
        public bool PlaylistSelected { get; set; }

        /// <summary>
        /// Occurs when the selection state changes.
        /// </summary>
        public event EventHandler PlaylistSelectionChangedEvent;

        /// <summary>
        /// Sets a new state.
        /// </summary>
        /// <param name="selected">The selection state.</param>
        public void PlaylistSelectionChanged(bool selected)
        {
            PlaylistSelected = selected;
            PlaylistSelectionChangedEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}