using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Azure.Documents.Spatial;




namespace TapThis.Model
{
    

    class Document
    {
        [JsonProperty(PropertyName = "id")]     
        public string Id { get; set; }

        [JsonProperty(PropertyName = "Bar_Id")]
        public string Bar_Id { get; set; }

        [JsonProperty(PropertyName = "BarName")]
        public string BarName { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "Price")]
        public string Price { get; set; }

        [JsonProperty(PropertyName = "Type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "Location")]
        public Point Location { get; set; }
        /*
        [JsonProperty(PropertyName = "Address")]
        public string Address { get; set; }
        */
        [JsonProperty(PropertyName = "Day")]
        public string Day { get; set; }

        [JsonProperty(PropertyName = "BleedDay")]
        public string BleedDay { get; set; }

        [JsonProperty(PropertyName = "Start")]
        public double Start { get; set; }

        [JsonProperty(PropertyName = "End")]
        public double End { get; set; }

        [JsonProperty(PropertyName = "BleedEnd")]
        public double BleedEnd { get; set; }

        [JsonProperty(PropertyName = "Address1")]
        public string Address1 { get; set; }

        [JsonProperty(PropertyName = "Address2")]
        public string Address2 { get; set; }

        
        [JsonProperty(PropertyName = "Open_Hours")]
        public string Open_Hours { get; set; }

        [JsonProperty(PropertyName = "Kitchen_Hours")]
        public string Kitchen_Hours { get; set; }
        

        [JsonProperty(PropertyName = "Phone")]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "Cover")]
        public bool? Cover { get; set; }



        //Kitchen Hours are still a thing

    }
}
