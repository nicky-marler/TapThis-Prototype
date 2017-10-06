using System;
using System.Collections.Generic;
using System.Text;
using MvvmHelpers;
using Xamarin.Forms.Maps;

namespace TapThis.Model
{
    public class Geo_Location : ObservableObject
    {
        public event EventHandler User_Location;

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        bool is_enabled = false;

        public void Connected()
        {
            On_User_Location(EventArgs.Empty);
        }

        protected virtual void On_User_Location(EventArgs e)
        {
            User_Location?.Invoke(this, e);
        }

        public bool Is_Enabled
        {
            get { return is_enabled; }
            set { SetProperty(ref is_enabled, value); }
        }

        public Position User_Position
        {
            get { return new Position(Latitude, Longitude); }
        }

        //Avoiding potential null. Set to Austin, Texas
        //public double Latitude = 30.267153;
        //public double Longitude = -97.743061;


    }
}
