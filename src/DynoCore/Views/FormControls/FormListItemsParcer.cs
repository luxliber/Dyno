using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Dyno.Views.FormControls
{
    public class FormListItemsParcer : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var list = new List<string>();

       //     foreach (var par in DynoManagerBase.SelectedWorkspacePreset.GetParameterByName(Parameters))
            {
       //         list.Add(par.Name);
            }

         

            return list;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

   
}