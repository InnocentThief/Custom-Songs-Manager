using CSM.DataAccess.BeatSaver;

namespace CSM.Business.Interfaces
{
    internal interface IBeatSaverService
    {
        Task<MapDetail?> GetMapDetailAsync(string id);

        Task<Dictionary<string, MapDetail>?> GetMapDetailsAsync(List<string> keys, BeatSaverKeyType keyType);

        Task<MapDetails?> SearchAsync(string query);
    }
}
