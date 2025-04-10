using CSM.DataAccess.UserConfiguration;

namespace CSM.Business.Interfaces
{
    internal interface IUserConfigDomain
    {
        string? TempDirectory { get; }

        UserConfig? Config { get; }

        void LoadOrCreateUserConfig();

        void SaveUserConfig();
    }
}
