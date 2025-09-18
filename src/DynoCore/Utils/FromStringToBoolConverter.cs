using System;
using System.Globalization;
using System.Windows.Data;

namespace Dyno.Utils
{
    public class FromStringToBoolConverter : IValueConverter
    {
      
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = value.ToString();

            if (String.IsNullOrEmpty(val))
                return false;

            if (val.ToLower() == "false")
                return false;

            if (val.ToLower() == "true")
                return true;
            
            double n;
            bool isNumeric = double.TryParse(val, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out n);    

            if (isNumeric)
                if (n > 0)
                    return true;
                else
                    return false;

           return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

   
}