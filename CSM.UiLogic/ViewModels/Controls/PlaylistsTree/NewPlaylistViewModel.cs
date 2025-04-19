using System.IO;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Controls.PlaylistsTree
{
    internal class NewPlaylistViewModel : BaseEditViewModel
    {
        private string playlistName = string.Empty;

        public override string Title => "Name of the new playlist";

        public string PlaylistName
        {
            get => playlistName;
            set
            {
                if (value == playlistName)
                    return;
                playlistName = value;
                OnPropertyChanged();
                ContinueCommand.RaiseCanExecuteChanged();
            }
        }

        public NewPlaylistViewModel(IServiceLocator serviceLocator, string cancelCommandText, EditViewModelCommandColor cancelCommandColor, string continueCommandText, EditViewModelCommandColor continueCommandColor) : base(serviceLocator, cancelCommandText, cancelCommandColor, continueCommandText, continueCommandColor)
        {
        }

        public override bool CanContinue()
        {
            var baseContinue = base.CanContinue();
            if (!baseContinue)
                return false;

            if (string.IsNullOrWhiteSpace(PlaylistName))
                return false;
            return PlaylistName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
        }
    }
}
