using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace TapThis.Model
{
    public class Bar_Pins : List<Pin>
    {

        public string Bar_List_Postion { get; set; }

        public event EventHandler Getting_Pins;

        public void Finshed_Getting_Pins()
        {
            On_Finshed_Getting_Pins(EventArgs.Empty);
        }

        protected virtual void On_Finshed_Getting_Pins(EventArgs e)
        {
            Getting_Pins?.Invoke(this, e);
        }

        

    }
}
