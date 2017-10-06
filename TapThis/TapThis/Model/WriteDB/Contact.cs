using System;
using System.Collections.Generic;
using System.Text;
using MvvmHelpers;
using Newtonsoft.Json;

namespace TapThis.Model.WriteDB
{
    public class Contact : ObservableObject
    {
        public string Document_Type { get; set; }

        public string First { get; set; }
        public string Last { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
