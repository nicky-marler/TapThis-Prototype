using System;
using System.Collections.Generic;
using System.Text;
using MvvmHelpers;

namespace TapThis.Model
{
    public class Time_HrMin : ObservableObject
    {
        int hour;
        int min;

        public int Hour
        {
            get { return hour; }
            set { SetProperty(ref hour, value); }
        }

        public int Min
        {
            get { return min; }
            set { SetProperty(ref min, value); }
        }


        public double Time_Double
        {
            get { return (double)Hour + ((double)Min / 60); }
        }
    }
}
