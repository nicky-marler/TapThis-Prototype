using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Globalization;

namespace TapThis.Converter
{
    public class EmptyListBool_Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value == 0 ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }
}
