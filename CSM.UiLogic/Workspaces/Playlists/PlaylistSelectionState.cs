using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM.UiLogic.Workspaces.Playlists
{
    public class PlaylistSelectionState
    {
        public bool PlaylistSelected { get; set; }

        public event EventHandler PlaylistSelectionChangedEvent;

        public void PlaylistSelectionChanged(bool selected)
        {
            PlaylistSelected = selected;
            PlaylistSelectionChangedEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}