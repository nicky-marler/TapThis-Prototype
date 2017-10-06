using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TapThis.Model
{
    class Send_Suggestion
    {
        
        public string Document_Type { get; set; }
        public string Business_Name { get; set; }
        public string Address { get; set; }
        public string City_State_Zip { get; set; }
        public string Country { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public List<Send_Item> Item_List
        {
            get; set;
        }

        public DateTime Date { get { return DateTime.Now; } }
  
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
