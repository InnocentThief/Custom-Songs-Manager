using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace CSM.UiLogic.Workspaces.Playlists
{
    /// <summary>
    /// Base class for a playlist entry (can be a folder or a playlist).
    /// </summary>
    public abstract class BasePlaylistViewModel : ObservableObject
    {
        /// <summary>
        /// Name of the playlist or folder.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new <see cref="BasePlaylistViewModel"/>.
        /// </summary>
        /// <param name="name">The name of the playlist or folder.</param>
        public BasePlaylistViewModel(string name)
        {
            Name = name;
        }
    }
}