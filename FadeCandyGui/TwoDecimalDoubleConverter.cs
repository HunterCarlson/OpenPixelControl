using System;
using System.Globalization;
using System.Windows.Data;

namespace FadeCandyGui
{
    public class TwoDecimalDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Binding.DoNothing;
            }
            return $"{value:0.##}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = value as string;
            if (data == null)
            {
                return value;
            }
            if (data.Equals(string.Empty))
            {
                return 0;
            }
            if (!string.IsNullOrEmpty(data))
            {
                double result;
                if (data.EndsWith(".") || data.Equals("-0"))
                {
                    return Binding.DoNothing;
                }
                if (double.TryParse(data, out result))
                {
                    return result;
                }
            }
            return Binding.DoNothing;
        }
    }
}