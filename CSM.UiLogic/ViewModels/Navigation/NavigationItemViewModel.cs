using CSM.Framework.ServiceLocation;
using CSM.Framework.Types;
using CSM.UiLogic.AbstractBase;

namespace CSM.UiLogic.ViewModels.Navigation
{
    internal sealed class NavigationItemViewModel(IServiceLocator serviceLocator, NavigationType navigationType, string displayName, string iconGlyph) : BaseViewModel(serviceLocator)
    {
        private bool isSelected;

        #region Public Properties

        public string DisplayName { get; set; } = displayName;

        public string IconGlyph { get; set; } = iconGlyph;

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (value == isSelected) return;
                isSelected = value;
                if (isSelected) SelectionChanged?.Invoke(this, EventArgs.Empty);
                OnPropertyChanged();
            }
        }

        public NavigationType NavigationType { get; } = navigationType;

        public Type? ViewModelType
        {
            get => NavigationType.ToViewModelType();
        }

        #endregion

        public event EventHandler? SelectionChanged;
    }
}
