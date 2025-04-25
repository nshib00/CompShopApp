using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace CompShop.Converters
{
    public class ImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = value as string;
            if (string.IsNullOrWhiteSpace(path))
                return null;

            try
            {
                // Абсолютный путь
                if (Path.IsPathRooted(path) && File.Exists(path))
                    return new BitmapImage(new Uri(path, UriKind.Absolute));

                // Относительный путь — предполагаем, что лежит рядом с .exe
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                var fullPath = System.IO.Path.Combine(basePath, path);

                if (File.Exists(fullPath))
                    return new BitmapImage(new Uri(fullPath, UriKind.Absolute));

                // Если не найден — fallback
                return null;
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
