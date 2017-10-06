using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Xamarin.Forms;

namespace TapThis.Converter
{
    class TimeSpanTimeObject : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Model.Time_HrMin Value = (Model.Time_HrMin)value;
            return new TimeSpan(Value.Hour, Value.Min, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan Time = (TimeSpan)value;
            Model.Time_HrMin Value = new Model.Time_HrMin { Hour = Time.Hours, Min = Time.Minutes };
            return Value;

        }

    }
}
