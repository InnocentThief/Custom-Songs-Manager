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

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(difficulty, DifficultyTypes.Easy, CompareOptions.IgnoreCase) == 0) return new SolidColorBrush(Color.FromArgb(255, 0, 128, 85));
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(difficulty, DifficultyTypes.Normal, CompareOptions.IgnoreCase) == 0) return new SolidColorBrush(Color.FromArgb(255, 18, 104, 161));
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(difficulty, DifficultyTypes.Hard, CompareOptions.IgnoreCase) == 0) return new SolidColorBrush(Color.FromArgb(255, 189, 85, 0));
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(difficulty, DifficultyTypes.Expert, CompareOptions.IgnoreCase) == 0) return new SolidColorBrush(Color.FromArgb(255, 181, 42, 28));
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(difficulty, DifficultyTypes.ExpertPlus, CompareOptions.IgnoreCase) == 0) return new SolidColorBrush(Color.FromArgb(255, 69, 64, 136));
            return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DifficultyNumberToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int difficulty = (int)value;

            if (difficulty == 1) return new SolidColorBrush(Color.FromArgb(255, 0, 128, 85));
            if (difficulty == 3) return new SolidColorBrush(Color.FromArgb(255, 18, 104, 161));
            if (difficulty == 5) return new SolidColorBrush(Color.FromArgb(255, 189, 85, 0));
            if (difficulty == 7) return new SolidColorBrush(Color.FromArgb(255, 181, 42, 28));
            if (difficulty == 9) return new SolidColorBrush(Color.FromArgb(255, 69, 64, 136));
            return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DifficultyToGradientStartColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string difficulty = value.ToString();

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(difficulty, DifficultyTypes.Easy, CompareOptions.IgnoreCase) == 0) return Color.FromArgb(50, 0, 128, 85);
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(difficulty, DifficultyTypes.Normal, CompareOptions.IgnoreCase) == 0) return Color.FromArgb(50, 18, 104, 161);
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(difficulty, DifficultyTypes.Hard, CompareOptions.IgnoreCase) == 0) return Color.FromArgb(50, 189, 85, 0);
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(difficulty, DifficultyTypes.Expert, CompareOptions.IgnoreCase) == 0) return Color.FromArgb(50, 181, 42, 28);
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(difficulty, DifficultyTypes.ExpertPlus, CompareOptions.IgnoreCase) == 0) return Color.FromArgb(50, 69, 64, 136);
            return Colors.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DifficultyToGradientStopColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string difficulty = value.ToString();

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(difficulty, DifficultyTypes.Easy, CompareOptions.IgnoreCase) == 0) return Color.FromArgb(150, 0, 128, 85);
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(difficulty, DifficultyTypes.Normal, CompareOptions.IgnoreCase) == 0) return Color.FromArgb(150, 18, 104, 161);
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(difficulty, DifficultyTypes.Hard, CompareOptions.IgnoreCase) == 0) return Color.FromArgb(150, 189, 85, 0);
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(difficulty, DifficultyTypes.Expert, CompareOptions.IgnoreCase) == 0) return Color.FromArgb(150, 181, 42, 28);
            if (CultureInfo.CurrentCulture.CompareInfo.Compare(difficulty, DifficultyTypes.ExpertPlus, CompareOptions.IgnoreCase) == 0) return Color.FromArgb(150, 69, 64, 136);
            return Colors.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}