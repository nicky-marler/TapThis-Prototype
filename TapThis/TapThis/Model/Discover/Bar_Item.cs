using System;
using Newtonsoft.Json;
using MvvmHelpers;

namespace TapThis.Model
{
    public class Bar_Item
    {
       // [JsonProperty(PropertyName = "id")]
       // public string Id { get; set; }

       // [JsonProperty(PropertyName = "name")]
        public string ItemName { get; set; }

       // [JsonProperty(PropertyName = "price")]
        public string Price { get; set; }

       // [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        public string Hours { get; set; }

        //public int Start { get; set; }
        //public int End { get; set; }

        public bool Active { get; set; }

        //Test
        // [JsonProperty(PropertyName = "bar_id")]
        // public string Bar_Id { get; set; }
    }
}
