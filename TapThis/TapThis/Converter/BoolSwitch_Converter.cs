using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Xamarin.Forms;


namespace TapThis.Converter
{
    public class BoolSwitch_Converter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;          
        }

    }
}
