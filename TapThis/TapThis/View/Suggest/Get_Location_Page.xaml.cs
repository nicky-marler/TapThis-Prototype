using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TapThis
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Get_Location_Page : ContentPage
	{

		public Get_Location_Page ()
		{
            InitializeComponent ();

            //Current_Location_Button.IsEnabled = App.User_Location.Is_Enabled;
            ((App)Application.Current).Suggest.Page_Navigation = Navigation;
            Current_Location_Button.BindingContext = App.User_Location;

        }

        private async void Mark_Map_Page(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Mark_Map_Page());
        }
    }
}