using System;
using System.Collections.Generic;
using System.Text;
using MvvmHelpers;
using Microsoft.Azure.Documents.Spatial;

namespace TapThis.Model
{
    public class Filter : ObservableObject
    {
 
        string day = DateTime.Today.DayOfWeek.ToString();
   //     int time = DateTime.Now.Hour;
        
        Model.Time_HrMin time = new Time_HrMin { Hour = DateTime.Now.Hour, Min = DateTime.Now.Minute };

        bool cover = true;
        string type = "All";
        string type2 = "Nothing";
        Point geo_location; //Add current user location. Worry when implementing maps.
        bool all_day = false;
        bool special = false;
        bool right_now = true;
        bool later_in_the_day= false;
        bool type_everything = true;
        bool type_food = true;
        bool type_drink = true;

        /*
        string day;
        int time;
        string type;
        Point geo_location; //Add current user location. Worry when implementing maps.
        bool all_day;
        bool special;
        */

        public string Day
        {
            get { return day; }
            set { SetProperty(ref day, value); }
        }
        /*
        public int Time
        {
            get { return time; }
            set { SetProperty(ref time, value); }
        }
    */    

        public string Type
        {
            get { return type; }
            set { SetProperty(ref type, value); }
        }

        public string Type2
        {
            get { return type2; }
            set { SetProperty(ref type2, value); }
        }

        public Point Geo_Location
        {
            get { return geo_location; }
            set { SetProperty(ref geo_location, value); }
        }
        public bool Special
        {
            get { return special; }
            set { SetProperty(ref special, value); }
        }
        public bool All_Day
        {
            get { return all_day; }
            set { SetProperty(ref all_day, value); }
        }
        public bool Right_Now
        {
            get { return right_now; }
            set { SetProperty(ref right_now, value); }
        }

        public bool Later_In_The_Day
        {
            get { return later_in_the_day; }
            set { SetProperty(ref later_in_the_day, value); }
        }

        public bool Type_Everything
        {
            get { return type_everything; }
            set { SetProperty(ref type_everything, value); }
        }
        public bool Type_Food
        {
            get { return type_food; }
            set { SetProperty(ref type_food, value); }
        }
        public bool Type_Drink
        {
            get { return type_drink; }
            set { SetProperty(ref type_drink, value); }
        }

        public bool Cover
        {
            get { return cover; }
            set { SetProperty(ref cover, value); }
        }

        public Model.Time_HrMin Time
        {
            get { return time; }
            set { SetProperty(ref time, value); }
        }
    }
}
