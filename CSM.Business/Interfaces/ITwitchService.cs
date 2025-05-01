namespace CSM.Business.Interfaces
{
    internal interface ITwitchService
    {
        Task GetAccessTokenAsync();
        Task<bool> ValidateAsync();
    }
}
