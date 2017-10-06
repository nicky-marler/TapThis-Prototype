using System;
using System.Collections.Generic;
using System.Text;
using MvvmHelpers;

namespace TapThis.Model
{
    public class Suggest_Item : ObservableObject
    {
        string item_name;
        string price;



        string type; 
        public List<string> Day = new List<string>();
        

        Model.Time_HrMin start = new Time_HrMin { Hour = 17, Min = 0 };
        Model.Time_HrMin end = new Time_HrMin { Hour = 19 , Min = 0 };

        public string Item_Name
        {
            get { return item_name; }
            set
            {
                SetProperty(ref item_name, value);
            }
        }

        public string Price
        {
            get { return price; }
            set
            {
                SetProperty(ref price, value);
            }
        }

        public string Type
        {
            get { return type; }
            set
            {
                SetProperty(ref type, value);
            }
        }

        public Model.Time_HrMin Start
        {
            get { return start; }
            set { SetProperty(ref start, value); }
        }

        public Model.Time_HrMin End
        {
            get { return end; }
            set { SetProperty(ref end, value); }
        }

    }
}
