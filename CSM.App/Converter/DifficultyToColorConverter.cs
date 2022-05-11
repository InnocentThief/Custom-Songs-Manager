using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CSM.App.Converter
{
    public class DifficultyToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string difficulty = value.ToString();
            switch (difficulty)
            {
                case "Easy":
                    return new SolidColorBrush(Colors.Green);
                case "Normal":
                    return new SolidColorBrush(Colors.CornflowerBlue);
                case "Hard":
                    return new SolidColorBrush(Colors.Orange);
                case "Expert":
                    return new SolidColorBrush(Colors.Red);
                case "ExpertPlus":
                    return new SolidColorBrush(Colors.MediumPurple);
                default:
                    return new SolidColorBrush(Colors.White);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}