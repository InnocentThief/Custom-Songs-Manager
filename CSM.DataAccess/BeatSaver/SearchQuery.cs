namespace CSM.DataAccess.BeatSaver
{
    internal class SearchQuery(string query, int pageIndex, bool isKey)
    {
        public string Query { get; } = query;
        public int PageIndex { get; } = pageIndex;
        public bool IsKey { get; } = isKey;
    }
}
