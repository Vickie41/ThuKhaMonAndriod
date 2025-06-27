// NullableIntConverter.cs
using System.Globalization;

namespace TKMApp.Converters
{
    public class NullableIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
                return null;

            if (int.TryParse(value.ToString(), out int result))
                return result;

            return null;
        }
    }
}

