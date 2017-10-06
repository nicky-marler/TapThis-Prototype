using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Azure.Documents.Spatial;
using MvvmHelpers;

namespace TapThis.Model
{
    public class Bar
    {

        //  [JsonProperty(PropertyName = "Bar_Id")]
        public string Bar_Id { get; set; }

        //  [JsonProperty(PropertyName = "BarName")]
        public string BarName { get; set; }

        // [JsonProperty(PropertyName = "Address")]


        // [JsonProperty(PropertyName = "Location")]
        public Point Location { get; set; }


        //public List<Bar_Item> Item_List { get; set; }

        public List<Grouped_List> Item_Group { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Open_Hours { get; set; }
        public string Kitchen_Hours { get; set; }
        public string Phone { get; set; }
        public bool? Cover { get; set; }





        //Raises property change event for the obserbale collection. Will cause slow runtime with 100+ Items
        /*
        public int Item_Count
        {
            get { return item_count; } 
            set { SetProperty(ref item_count, value); }
        }
        */

    }
}
