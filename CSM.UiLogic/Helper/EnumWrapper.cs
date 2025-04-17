using CSM.Framework.ServiceLocation;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.Helper
{
    internal sealed class EnumWrapper<TEnum> : BaseViewModel where TEnum : Enum
    {
        public string DisplayText { get; }

        public TEnum Value { get; }

        public EnumWrapper(IServiceLocator serviceLocator, TEnum enumValue) : base(serviceLocator)
        {
            DisplayText = UiText.GetText(enumValue);
            Value = enumValue;
        }

        internal static IEnumerable<EnumWrapper<TEnum>> GetValues(IServiceLocator serviceLocator)
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>()
                .Select(e => new EnumWrapper<TEnum>(serviceLocator, e))
                .OrderBy(e => e.DisplayText);
        }

        internal static IEnumerable<EnumWrapper<TEnum>> GetValues<TOrder>(IServiceLocator serviceLocator, Func<EnumWrapper<TEnum>, TOrder> orderBy)
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>()
                .Select(e => new EnumWrapper<TEnum>(serviceLocator, e))
                .OrderBy(orderBy);
        }

        internal static IEnumerable<EnumWrapper<TEnum>> GetValues(IServiceLocator serviceLocator, params TEnum[] excludedValues)
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>()
                .Except(excludedValues)
                .Select(e => new EnumWrapper<TEnum>(serviceLocator, e))
                .OrderBy(e => e.DisplayText);
        }
    }
}
