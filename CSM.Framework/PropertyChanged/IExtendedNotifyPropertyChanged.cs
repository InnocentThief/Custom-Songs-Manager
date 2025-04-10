using System.ComponentModel;

namespace CSM.Framework.PropertyChanged
{
    internal interface IExtendedNotifyPropertyChanged : INotifyPropertyChanged
    {
        void OnPropertyChanged(string? propertyName);
    }
}
