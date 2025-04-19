using CSM.Business.Core.SongSelection;

namespace CSM.Business.Interfaces
{
    internal interface ISongSelectionDomain
    {
        event EventHandler<SongSelectionChangedEventArgs>? OnSongSelectionChanged;

        void SetSongHash(string? hash, SongSelectionType songSelectionType);
    }
}
