using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;
using MvvmHelpers;

namespace TapThis
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Mark_Map_Page : ContentPage
    {
        public Pin Marked_Pin { get { return ((App)Application.Current).Suggest.Mark_Pin; } }

		public Mark_Map_Page ()
		{
			InitializeComponent ();
            Confirm_Button.IsEnabled = false;
            Set_Map_Default();
            ((App)Application.Current).Suggest.Marking_Pin += Mark_Map_Pin;

        }

        public void Set_Map_Default()
        {
            Distance Standard_User_Zoom = Distance.FromKilometers(10);
            MapSpan Goto_User = MapSpan.FromCenterAndRadius(App.User_Location.User_Position, Standard_User_Zoom);
            User_Map.MoveToRegion(Goto_User);

        }


        private void Mark_Map_Pin(object sender, EventArgs e)
        {
   
            Confirm_Button.IsEnabled = true;
            User_Map.Pins.Clear();
               
            User_Map.Pins.Add(((App)Application.Current).Suggest.Mark_Pin);

            
        }

    }
}