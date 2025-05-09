namespace CSM.Business.Interfaces
{
    internal interface IBeatLeaderService
    {
        Task<bool> PlayerExistsAsync(string id);
    }
}
