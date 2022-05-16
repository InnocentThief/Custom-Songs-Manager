using CSM.DataAccess.Entities.Types;
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

            if (difficulty == DifficultyTypes.Easy) return new SolidColorBrush(Color.FromArgb(255, 0, 128, 85));
            if (difficulty == DifficultyTypes.Normal) return new SolidColorBrush(Color.FromArgb(255, 18, 104, 161));
            if (difficulty == DifficultyTypes.Hard) return new SolidColorBrush(Color.FromArgb(255, 189, 85, 0));
            if (difficulty == DifficultyTypes.Expert) return new SolidColorBrush(Color.FromArgb(255, 181, 42, 28));
            if (difficulty == DifficultyTypes.ExpertPlus) return new SolidColorBrush(Color.FromArgb(255, 69, 64, 136));
            return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}