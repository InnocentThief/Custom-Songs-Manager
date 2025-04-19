using System.IO;
using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Controls.PlaylistsTree
{
    internal class NewPlaylistFolderViewModel(IServiceLocator serviceLocator, string cancelCommandText, EditViewModelCommandColor cancelCommandColor, string continueCommandText, EditViewModelCommandColor continueCommandColor) : BaseEditViewModel(serviceLocator, cancelCommandText, cancelCommandColor, continueCommandText, continueCommandColor)
    {
        private string folderName = string.Empty;

        public override string Title => "Name of the new playlist folder";

        public string FolderName
        {
            get => folderName;
            set
            {
                if (value == folderName)
                    return;
                folderName = value;
                OnPropertyChanged();

                ContinueCommand.RaiseCanExecuteChanged();
            }
        }

        public override bool CanContinue()
        {
            var baseContinue =  base.CanContinue();
            if (!baseContinue)
                return false;

            if (string.IsNullOrWhiteSpace(FolderName)) 
                return false;
            return FolderName.IndexOfAny(Path.GetInvalidPathChars()) < 0;
        }
    }
}
