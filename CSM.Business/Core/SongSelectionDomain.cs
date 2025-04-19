using CSM.Business.Core.SongSelection;
using CSM.Business.Interfaces;

namespace CSM.Business.Core
{
    internal class SongSelectionDomain : ISongSelectionDomain
    {
        public event EventHandler<SongSelectionChangedEventArgs>? OnSongSelectionChanged;

        public void SetSongHash(string? hash, SongSelectionType songSelectionType)
        {
            OnSongSelectionChanged?.Invoke(this, new SongSelectionChangedEventArgs
            {
                SongSelectionType = songSelectionType,
                SongHash = hash
            });
        }
    }
}
