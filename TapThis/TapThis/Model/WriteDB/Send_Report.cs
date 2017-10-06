using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TapThis.Model
{
    class Send_Report
    {
        public string Document_Type { get; set; }
        public string Bar_Id { get; set; }
        public string Item_Name { get; set; }
        public string Reason { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
