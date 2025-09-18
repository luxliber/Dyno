using System;
using System.Globalization;
using System.Windows.Data;

namespace Prorubim.DynoStudio.Utils.Converters
{
    public class ZoomConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var factor = value as double? ?? 1.0;

            return $"{factor*100}%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value.ToString().TrimEnd('%');
            double factor;
            double.TryParse(str, out factor);

            return factor/100;
        }
    }
}