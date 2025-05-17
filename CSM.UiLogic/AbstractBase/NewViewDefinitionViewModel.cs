using CSM.Framework.ServiceLocation;
using System.IO;

namespace CSM.UiLogic.AbstractBase
{
    internal class NewViewDefinitionViewModel(
        IServiceLocator serviceLocator,
        string cancelCommandText,
        EditViewModelCommandColor cancelCommandColor,
        string continueCommandText,
        EditViewModelCommandColor continueCommandColor)
        : BaseEditViewModel(serviceLocator, cancelCommandText, cancelCommandColor, continueCommandText, continueCommandColor)
    {
        private string viewDefinitionName = string.Empty;

        public override string Title => "Name of the new view definition";

        public string ViewDefinitionName
        {
            get => viewDefinitionName;
            set
            {
                if (value == viewDefinitionName)
                    return;
                viewDefinitionName = value;
                OnPropertyChanged();
                ContinueCommand.RaiseCanExecuteChanged();
            }
        }

        public override bool CanContinue()
        {
            var baseContinue = base.CanContinue();
            if (!baseContinue)
                return false;

            if (string.IsNullOrWhiteSpace(ViewDefinitionName))
                return false;
            return ViewDefinitionName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
        }
    }
}
