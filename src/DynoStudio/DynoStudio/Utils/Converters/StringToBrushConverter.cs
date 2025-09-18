using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Prorubim.DynoStudio.Utils.Converters
{
    [ValueConversion(typeof(string), typeof(Brush))]
    public class StringToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = ColorConverter.ConvertFromString((string)value);
            return color != null ? new SolidColorBrush((Color)color) : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}