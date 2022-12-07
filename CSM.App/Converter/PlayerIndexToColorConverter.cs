using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CSM.App.Converter
{
    public class PlayerIndexToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int index = (int)value;
            switch (index)
            {
                case 0: return new SolidColorBrush(Color.FromArgb(255, 100, 149, 237));
                case 1: return new SolidColorBrush(Color.FromArgb(255, 144, 100, 237));
                case 2: return new SolidColorBrush(Color.FromArgb(255, 100, 237, 189));
                case 3: return new SolidColorBrush(Color.FromArgb(255, 237, 191, 100));
                case 4: return new SolidColorBrush( Color.FromArgb(255, 237, 123, 100));
                case 5: return new SolidColorBrush(Color.FromArgb(255, 212, 100, 237));
                default: return new SolidColorBrush(Color.FromArgb(255, 100, 149, 237));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}