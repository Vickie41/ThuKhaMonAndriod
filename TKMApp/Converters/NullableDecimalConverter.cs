// NullableDecimalConverter.cs
using System.Globalization;

namespace TKMApp.Converters
{
    public class NullableDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
                return null;

            if (decimal.TryParse(value.ToString(), out decimal result))
                return result;

            return null;
        }
    }
}