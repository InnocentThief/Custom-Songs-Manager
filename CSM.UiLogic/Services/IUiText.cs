namespace CSM.UiLogic.Services
{
    internal interface IUiText
    {
        string GetText(string key);

        string GetText(Enum e);
    }
}
