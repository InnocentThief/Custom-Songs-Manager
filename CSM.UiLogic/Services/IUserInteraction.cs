using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.Services
{
    internal interface IUserInteraction
    {
        void ShowError(string message);

        void ShowWarning(string message);

        void ShowWindow<T>(T value) where T : BaseEditViewModel;
    }
}
