using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TapThis.Model
{
    class Send_Comment
    {
        public string Document_Type { get; set; }
        public string Reason { get; set; }
        public string Comment { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
