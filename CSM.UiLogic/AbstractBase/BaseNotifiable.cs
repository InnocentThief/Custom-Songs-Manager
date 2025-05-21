using CSM.Framework.PropertyChanged;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CSM.UiLogic.AbstractBase
{
    public class BaseNotifiable : IExtendedNotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            OnPropertyChanged(this, propertyName);
        }

        public void OnPropertyChanged(object? sender, string? propertyName)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }
    }
}
