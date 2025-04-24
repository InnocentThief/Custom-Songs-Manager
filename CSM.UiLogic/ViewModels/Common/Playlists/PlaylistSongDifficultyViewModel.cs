using CSM.DataAccess.Common;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Common.Playlists
{
    internal class PlaylistSongDifficultyViewModel : BaseViewModel
    {
        private readonly DataAccess.Playlists.Difficulty difficulty;

        public Characteristic Characteristic => difficulty.Characteristic;

        public Difficulty Difficulty => difficulty.Name;

        public PlaylistSongDifficultyViewModel(IServiceLocator serviceLocator, DataAccess.Playlists.Difficulty difficulty) : base(serviceLocator)
        {
            this.difficulty = difficulty;
        }
    }
}
