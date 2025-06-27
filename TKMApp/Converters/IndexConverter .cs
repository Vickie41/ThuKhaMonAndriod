using System.Globalization;
using TKMApp.ViewModels;


namespace TKMApp.Converters
{
    public class IndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MedicineItem item && parameter is CollectionView collectionView)
            {
                var index = collectionView.ItemsSource.Cast<object>().ToList().IndexOf(item) + 1;
                return $"Medicine {index}";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}