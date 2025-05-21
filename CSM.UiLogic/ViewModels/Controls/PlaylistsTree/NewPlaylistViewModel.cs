using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;
using System.IO;

namespace CSM.UiLogic.ViewModels.Controls.PlaylistsTree
{
    internal class NewPlaylistViewModel(
        IServiceLocator serviceLocator,
        string cancelCommandText,
        EditViewModelCommandColor cancelCommandColor,
        string continueCommandText,
        EditViewModelCommandColor continueCommandColor)
        : BaseEditViewModel(serviceLocator, cancelCommandText, cancelCommandColor, continueCommandText, continueCommandColor)
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
