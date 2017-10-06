using System;
using System.Collections.Generic;
using System.Text;

namespace TapThis.Model
{
    public class Send_Item
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public string Type { get; set; }
        public double Start { get; set; }
        public double End { get; set; }
        public List<string> Day { get; set; }


    }
}
