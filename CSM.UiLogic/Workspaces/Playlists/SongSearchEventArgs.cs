using System;

namespace CSM.UiLogic.Workspaces.Playlists
{
    public class SongSearchEventArgs: EventArgs
    {
        public string SearchString { get; }

        public SongSearchEventArgs(string searchString)
        {
            SearchString = searchString;
        }
    }
}