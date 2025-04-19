namespace CSM.Business.Core.SongSelection
{
    internal class SongSelectionChangedEventArgs : EventArgs
    {
        public SongSelectionType SongSelectionType { get; set; }
        public string? SongHash { get; set; }
    }
}
