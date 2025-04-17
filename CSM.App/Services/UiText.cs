using CSM.App.Properties;
using CSM.UiLogic.Services;

namespace CSM.App.Services
{
    internal class UiText : IUiText
    {
        public string GetText(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return string.Empty;

            var propertyInfo = typeof(Resources).GetProperty(key);
            if (propertyInfo == null)
                return string.Format("!{0}!", key);

            return propertyInfo.GetValue(new Resources()) as string ?? string.Format("!{0}!", key);
        }

        public string GetText(Enum e)
        {
            var enumType = e.GetType().Name;
            var textName = string.Format("enum_{0}_{1}", enumType, Enum.GetName(e.GetType(), e));
            return GetText(textName);
        }
    }
}
