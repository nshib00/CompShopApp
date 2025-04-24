using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace CompShop.Converters
{
    public class ImagePathConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? path = value as string;
            if (!string.IsNullOrWhiteSpace(path))
            {
                if (File.Exists(path))
                {
                    return new BitmapImage(new Uri(path, UriKind.Absolute));
                }
            }

            return null; // триггерит TextBlock “(нет изображения)”
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}

