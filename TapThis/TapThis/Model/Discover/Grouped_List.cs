using System;
using System.Collections.Generic;
using System.Text;

namespace TapThis.Model
{
    public class Grouped_List : List<Bar_Item>
    {       
        public string Title { get; set; }
        public string ShortName { get; set; }
  
    }
}
