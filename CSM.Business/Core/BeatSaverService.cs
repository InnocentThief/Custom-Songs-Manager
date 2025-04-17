using CSM.Business.Interfaces;

namespace CSM.Business.Core
{
    internal class BeatSaverService(IUserConfigDomain userConfigDomain) : IBeatSaverService
    {
        private readonly GenericServiceClient client = new GenericServiceClient(userConfigDomain.Config?.BeatSaverAPIEndpoint ?? "https://api.beatsaver.com/");

    }
}
